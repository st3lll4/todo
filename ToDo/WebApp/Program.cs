using System.Net;
using System.Text.Json.Serialization;
using BLL.Contracts;
using BLL.Services;
using DAL;
using DAL.Contracts;
using DAL.DataSeeding;
using DAL.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Npgsql;
using WebApp.Models;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddControllers(options =>
    {
        options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
    })
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });


if (builder.Environment.IsProduction())
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(
            connectionString,
            o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery
            ) 
        )
    );
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(
                connectionString,
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery
                )
            )
            .ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning))
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging() 
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
    );
}

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<ITaskListRepository, TaskListRepository>();
builder.Services.AddScoped<IListItemRepository, ListItemRepository>();

builder.Services.AddScoped<ITaskListService, TaskListService>();
builder.Services.AddScoped<IListItemService, ListItemService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsAllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
        policy.SetIsOriginAllowed((host) => true);
    });
});

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

await SetupAppData(app, app.Configuration);

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
}
else
{
    // catch any unhandled exceptions and convert them to generic messages
    app.UseExceptionHandler(appError =>
    {
        appError.Run(async context =>
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature != null)
            {
                await context.Response.WriteAsync(new ErrorViewModel
                {
                    Code = context.Response.StatusCode,
                    Message = "Internal Server Error."
                }.ToString() ?? "");
            }
        });
    });
}

app.UseCors("CorsAllowAll");
app.UseRouting();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

static async Task SetupAppData(IApplicationBuilder app, IConfiguration configuration)
{
    // Enables access to scoped services during startup
    using var serviceScope = app.ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope();
    var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<IApplicationBuilder>>();
    var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

    WaitDbConnection(context, logger);

    if (context.Database.ProviderName!.Contains("InMemory"))
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return;
    }

    if (configuration.GetValue<bool>("DataInitialization:DropDatabase"))
    {
        logger.LogWarning("Drop db");
        AppDataInit.DeleteDatabase(context);
    }

    if (configuration.GetValue<bool>("DataInitialization:MigrateDatabase"))
    {
        logger.LogInformation("Migrate db");
        AppDataInit.MigrateDatabase(context);
    }

    if (configuration.GetValue<bool>("DataInitialization:SeedData"))
    {
        logger.LogInformation("SeedData");
        await AppDataInit.SeedData(context);
    }
}

static void WaitDbConnection(AppDbContext ctx, ILogger<IApplicationBuilder> logger)
{
    var maxRetries = 30;
    var retryCount = 0;

    while (retryCount < maxRetries)
    {
        try
        {
            ctx.Database.OpenConnection();
            ctx.Database.CloseConnection();
            logger.LogInformation("Database connection successful");
            return;
        }
        catch (PostgresException e)
        {
            logger.LogWarning("Checked postgres db connection. Got: {Message}", e.Message);

            if (e.Message.Contains("does not exist"))
            {
                logger.LogWarning("Database does not exist. Will attempt to create via migrations.");
                return;
            }

            retryCount++;
            logger.LogWarning("Waiting for db connection. Retry {RetryCount}/{MaxRetries}. Sleep 2 sec", retryCount,
                maxRetries);
            Thread.Sleep(2000);
        }
        catch (Exception e)
        {
            retryCount++;
            logger.LogWarning("Database connection failed with: {Message}. Retry {RetryCount}/{MaxRetries}", e.Message,
                retryCount, maxRetries);
            Thread.Sleep(2000);
        }
    }

    throw new InvalidOperationException($"Could not connect to database after {maxRetries} attempts");
}