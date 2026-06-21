using Firma_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Firma_API.Data
{
    public class CompanyApiDbContext : DbContext
    {
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Division> Divisions => Set<Division>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Department> Departments => Set<Department>();


        public CompanyApiDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Firma
            modelBuilder.Entity<Company>(entityCompany =>
            {
                entityCompany.HasKey(comp => comp.Id);
                entityCompany.Property(comp => comp.Name).IsRequired().HasMaxLength(200);
                entityCompany.Property(comp => comp.Code).IsRequired().HasMaxLength(50);
                entityCompany.HasIndex(comp => comp.Code).IsUnique();

                entityCompany.HasOne(comp => comp.Director)
                 .WithMany()
                 .HasForeignKey(comp => comp.DirectorId)
                 .OnDelete(DeleteBehavior.SetNull);

                entityCompany.HasMany(comp => comp.Employees)
                 .WithOne(comp => comp.Company)
                 .HasForeignKey(comp => comp.CompanyId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            //Zamestnanec
            modelBuilder.Entity<Employee>(employeeEntity =>
            {
                employeeEntity.HasKey(emp => emp.Id);
                employeeEntity.Property(emp => emp.FirstName).IsRequired().HasMaxLength(100);
                employeeEntity.Property(emp => emp.LastName).IsRequired().HasMaxLength(100);
                employeeEntity.Property(emp => emp.Phone).IsRequired().HasMaxLength(30);
                employeeEntity.Property(emp => emp.Email).IsRequired().HasMaxLength(200);
                employeeEntity.Property(emp => emp.Title).HasMaxLength(50);
            });

            //Divízia
            modelBuilder.Entity<Division>(divisionEntity =>
            {
                divisionEntity.HasKey(div => div.Id);
                divisionEntity.Property(div => div.Name).IsRequired().HasMaxLength(200);
                divisionEntity.Property(div => div.Code).IsRequired().HasMaxLength(50);

                divisionEntity.HasOne(div => div.Company)
                 .WithMany(div => div.Divisions)
                 .HasForeignKey(div => div.CompanyId)
                 .OnDelete(DeleteBehavior.Cascade);

                divisionEntity.HasOne(div => div.Leader)
                 .WithMany()
                 .HasForeignKey(div => div.LeaderId)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            //Projekt
            modelBuilder.Entity<Project>(projectEntity =>
            {
                projectEntity.HasKey(proj => proj.Id);
                projectEntity.Property(proj => proj.Name).IsRequired().HasMaxLength(200);
                projectEntity.Property(proj => proj.Code).IsRequired().HasMaxLength(50);

                projectEntity.HasOne(proj => proj.Division)
                 .WithMany(proj => proj.Projects)
                 .HasForeignKey(proj => proj.DivisionId)
                 .OnDelete(DeleteBehavior.Cascade);

                projectEntity.HasOne(proj => proj.Leader)
                 .WithMany()
                 .HasForeignKey(proj => proj.LeaderId)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            //Oddelenie
            modelBuilder.Entity<Department>(departmentEntity =>
            {
                departmentEntity.HasKey(depart => depart.Id);
                departmentEntity.Property(depart => depart.Name).IsRequired().HasMaxLength(200);
                departmentEntity.Property(depart => depart.Code).IsRequired().HasMaxLength(50);

                departmentEntity.HasOne(depart => depart.Project)
                 .WithMany(x => x.Departments)
                 .HasForeignKey(depart => depart.ProjectId)
                 .OnDelete(DeleteBehavior.Cascade);

                departmentEntity.HasOne(depart => depart.Leader)
                 .WithMany()
                 .HasForeignKey(depart => depart.LeaderId)
                 .OnDelete(DeleteBehavior.SetNull);
            });

        }
    }
}
