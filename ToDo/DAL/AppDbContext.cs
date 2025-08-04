using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options,
        ILogger<AppDbContext> logger)
        : base(options)
    {
        _logger = logger;
    }

    public DbSet<TaskList> TaskLists { get; set; } = default!;
    public DbSet<ListItem> ListItems { get; set; } = default!;

    private readonly ILogger<AppDbContext> _logger;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ListItem>()
            .Property(cp => cp.Priority)
            .HasConversion<string>();

        modelBuilder.Entity<TaskList>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ListItem>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var entries = ChangeTracker.Entries();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Property("CreatedAt").IsModified = false;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}