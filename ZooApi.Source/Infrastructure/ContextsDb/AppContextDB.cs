using DomainAnimal.Entities;
using DomainAnimal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ContextsDb
{
    public class AppContextDB : DbContext
    {
        public AppContextDB(DbContextOptions<AppContextDB> options) : base(options) { }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка модели (таблицы в БД) животных
            modelBuilder
                .Entity<Animal>()
                .ToTable("animals")
                .HasDiscriminator<string>("animal_type")
                .HasValue<Lion>("Lion")
                .HasValue<Monkey>("Monkey");

            modelBuilder
                .Entity<Animal>()
                .HasKey(X => X.Id);

            modelBuilder
                .Entity<Animal>()
                .Property(x => x.Id)
                .HasColumnName("id");

            modelBuilder
                .Entity<Animal>()
                .Property(x => x.Name)
                .HasColumnName("name");

            modelBuilder
                .Entity<Animal>()
                .Property(x => x.Energy)
                .HasColumnName("energy");

            modelBuilder
                .Entity<Animal>()
                .Property(x => x.SomeSecretInformation)
                .HasColumnName("secret_information")
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder
                .Entity<Animal>()
                .HasOne(x => x.Employee)
                .WithMany(x => x.Animals)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull);
                
            modelBuilder
                .Entity<Animal>()
                .Property(x => x.EmployeeId)
                .HasColumnName("employee_id");

            // Настройка модели (таблицы в БД) сотрудников
            modelBuilder
                .Entity<Employee>()
                .ToTable("employees");

            modelBuilder
                .Entity<Employee>()
                .HasKey(X => X.Id);

            modelBuilder 
                .Entity<Employee>()
                .Property(x => x.Id)
                .HasColumnName("id");

            modelBuilder
                .Entity<Employee>()
                .Property(x => x.Name)
                .HasColumnName("name");

            modelBuilder
                .Entity<Employee>()
                .Property(x => x.Position)
                .HasConversion<string>()
                .HasColumnName("position");

            modelBuilder
                .Entity<Employee>()
                .Property(x => x.Limit)
                .HasColumnName("animal_limit");

            modelBuilder
                .Entity<Employee>()
                .Property(x => x.Balance)
                .HasColumnName("balance");
        }
    }
}
