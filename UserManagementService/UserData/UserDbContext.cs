using Microsoft.EntityFrameworkCore;
using UserData.Entities;

namespace UserData;

public class UserDbContext : DbContext
{
    public UserDbContext()
    {
    }

    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    public virtual DbSet<User> User { get; set; } = null!;
    public virtual DbSet<UserActionLog> UserActionLog { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserActionLog>()
            .HasKey(e => e.ActionLogId);
        
        modelBuilder.Entity<User>()
            .HasMany(e => e.UserActionLogs)
            .WithOne(e => e.Usr)
            .HasForeignKey(e => e.UserId)
            .IsRequired();     
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        
        var connectionString = "";

        if (environment == "Development")
        {
            connectionString =
                "Server=localhost;" +
                "Initial Catalog=UserDbDevelopment;" + 
                "Integrated Security=False;" +
                "User Id=sa;" +
                "Password=MyPass@word;" +
                "TrustServerCertificate=True";
        }
        else if (environment == "Production")
        {
            connectionString =
                "Server=localhost;" +
                "Initial Catalog=UserDb;" + 
                "Integrated Security=False;" +
                "User Id=sa;" +
                "Password=MyPass@word;" +
                "TrustServerCertificate=True";
        }
        else
        {
            throw new InvalidOperationException("Unknown environment: " + environment);
        }

        optionsBuilder.UseSqlServer(connectionString);
    }
}
