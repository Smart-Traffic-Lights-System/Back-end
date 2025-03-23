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
    public virtual DbSet<UserRole> UserRole { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRole>()
            .HasOne(e => e.Usr)
            .WithOne(e => e.Role)
            .HasForeignKey("RoleId")
            .IsRequired();
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.UserActionLogs)  // A User has many UserActionLogs
            .WithOne(log => log.Usr)        // Each UserActionLog belongs to one User
            .HasForeignKey(log => log.Usr.UserId)  // Foreign Key in UserActionLog
            .HasPrincipalKey(u => u.UserId);       // Primary Key in User
    }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var connectionString = "";

        if (environment == "Development")
        {
            connectionString =
                "Server=localhost,1433;" +
                "Initial Catalog=UserDbDevelopment;" + 
                "Integrated Security=False;" +
                "User Id=sa;" +
                "Password=MyPass@word;" +
                "TrustServerCertificate=True";
        }
        else if (environment == "Production")
        {
            connectionString =
                "Server=localhost,1433;" +
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
