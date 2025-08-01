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
    }
}