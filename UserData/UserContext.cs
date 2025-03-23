using Microsoft.EntityFrameworkCore;
using UserData.Entities;

namespace UserData;

public class UserContext : DbContext
{
    public UserContext()
    {
    }

    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }

    public virtual DbSet<User> User { get; set; } = null!;
    public virtual DbSet<UserActionLog> UserActionLog { get; set; } = null!;
    public virtual DbSet<UserRole> UserRole { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserActionLog>()
            .HasKey(e => e.ActionLogId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var connectionString = "";

        if (environment == "Development")
        {
            connectionString =
                "server=DESKTOP-FG9B3DG\\SQLEXPRESS;" +   
                "database=UserDatabase_Development;" +
                "Integrated Security=True;" +
                "MultipleActiveResultSets=True;" +
                "TrustServerCertificate=True";
        }
        else if (environment == "Production")
        {
            connectionString =
                "server=DESKTOP-FG9B3DG\\SQLEXPRESS;" +   
                "database=UserDatabase_Production;" +
                "Integrated Security=True;" +
                "MultipleActiveResultSets=True;" +
                "TrustServerCertificate=True";
        }
        else
        {
            throw new InvalidOperationException("Unknown environment: " + environment);
        }

        optionsBuilder.UseSqlServer(connectionString);
    }
}
