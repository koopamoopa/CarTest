using Microsoft.EntityFrameworkCore;
using ProjectCarTest.Models;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<CarInfo> CarInfos { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.userID);
        modelBuilder.Entity<CarInfo>().HasKey(c => c.carID);
        modelBuilder.Entity<CarInfo>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.userID)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<CarInfo>()
            .HasIndex(c => new { c.userID, c.make, c.model, c.year })
            .IsUnique();
    }
}
