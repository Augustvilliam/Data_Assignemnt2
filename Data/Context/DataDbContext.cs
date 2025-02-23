using System.Diagnostics;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class DataDbContext : DbContext
{
    public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
    {
        Debug.WriteLine("DataDbContext skapad"); //debug från när databasen inte ville skapas, låter den ligga kvar. 
    }

    public DbSet<EmployeeEntity> Employees { get; set; }
    public DbSet<ServiceEntity> Services { get; set; }
    public DbSet<RoleDto> Roles { get; set; }
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<ProjectEntity> Projects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeEntity>() //deklarera många till många relationer. 
            .HasMany(e => e.Services)
            .WithMany(s => s.Employees)
            .UsingEntity<Dictionary<string, object>>(
                "EmployeeService",
                j => j.HasOne<ServiceEntity>().WithMany().HasForeignKey("ServiceId"),
                j => j.HasOne<EmployeeEntity>().WithMany().HasForeignKey("EmployeeId"),
                j =>
                {
                    j.HasKey("EmployeeId", "ServiceId");
                    j.ToTable("EmployeeService");
                });

        modelBuilder.Entity<RoleDto>().HasData( //hårdkodade Roller och services eftersom det blev för många steg att skapa en Employee och senare projekt. Genererad av chatGPT
            new RoleDto { Id = 1, Name = "Intern", Price = 100 },
            new RoleDto { Id = 2, Name = "Junior", Price = 200 },
            new RoleDto { Id = 3, Name = "Senior", Price = 400 }
        );

        modelBuilder.Entity<ServiceEntity>().HasData( //som tidigare nämt är EstimatedHours i princip en restprodukt i detta skedet. 
            new ServiceEntity { Id = 1, Name = "Consultation", BasePrice = 500, EstimatedHours = 2 },
            new ServiceEntity { Id = 2, Name = "Development", BasePrice = 1500, EstimatedHours = 10 },
            new ServiceEntity { Id = 3, Name = "Testing", BasePrice = 700, EstimatedHours = 5 }
        );

        base.OnModelCreating(modelBuilder);
    }
    //Gamla medtoden för att slänga in Roller innan jag byggde om skiten. behåller den som kopmmentar i dokumentationssyfte. 
    //public void SeedRoles() 
    //{
    //    if (!Roles.Any())
    //    {
    //        Roles.AddRange(new List<RoleEntity>
    //        {
    //            new() { Id = 1, Name = "Intern", Price = 100},
    //            new() { Id = 2, Name = "Junior", Price = 200},
    //            new() { Id = 3, Name = "Senior", Price = 400}
    //        });
    //        SaveChanges();
    //        Debug.WriteLine("Roller har lagts till i databasen.");
    //    }

}
