
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class DataDbContext : DbContext
{
    public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
    {
        Database.EnsureDeleted(); //TA BORT MIG SEN NÄR ALLT FUNGERAR!!!
        Database.EnsureCreated();
    }

    public DbSet<ProjectEntity> Projects { get; set; }
    public DbSet<ServiceEntity> Services { get; set; }
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<EmployeeEntity> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeEntity>()
            .HasOne(e => e.Role)
            .WithMany(r => r.Employees)
            .HasForeignKey(e => e.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.Employee)
            .WithMany(e => e.Projects)
            .HasForeignKey(p => p.EmployeeId);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.Customer)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.CustomerId);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.Service)
            .WithMany(b => b.Projects)
            .HasForeignKey(p => p.ServiceId);
    }



}
