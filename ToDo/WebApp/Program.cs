using System.Text.Json.Serialization;
using DAL;
using DAL.Contracts;
using DAL.Mappers;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Npgsql;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// used for older style [Column(TypeName = "jsonb")] for LangStr
#pragma warning disable CS0618 // Type or member is obsolete
NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();
#pragma warning restore CS0618 // Type or member is obsolete

builder.Services.AddControllers(options =>
    {
        options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); 
    });


if (builder.Environment.IsProduction())
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(
            connectionString,
            o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery
            ) // optimization by npgsql
        )
    );
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(
                connectionString,
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery
                ) // optimization by npgsql
            )
            .ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning))
            // punishes you for querying a cartesian explosion and creates a warning 
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging() // see values that are used in sql queries
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
    );
}

builder.Services.AddDatabaseDeveloperPageExceptionFilter();



builder.Services.AddScoped<ITaskListRepository, TaskListRepository>();
builder.Services.AddScoped<IListItemRepository, ListItemRepository>();


builder.Services.AddControllersWithViews();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler();
}

app.UseRouting();

app.UseCors("CorsAllowAll");

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();