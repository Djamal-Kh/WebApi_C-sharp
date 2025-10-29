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
                .ToTable("AnimalsOfZoo")
                .HasDiscriminator<string>("AnimalType")
                .HasValue<Lion>("Lion")
                .HasValue<Monkey>("Monkey");

            modelBuilder
                .Entity<Animal>()
                .HasKey(X => X.Id);

            modelBuilder
                .Entity<Animal>()
                .Property(x => x.Name)
                .HasColumnName("Name");

            modelBuilder
                .Entity<Animal>()
                .Property(x => x.Energy)
                .HasColumnName("EnergyOfAnimal");

            modelBuilder
                .Entity<Animal>()
                .Property(x => x.Type)
                .HasConversion<string>();

            modelBuilder
                .Entity<Animal>()
                .Property(x => x.SomeSecretInformation)
                .HasColumnName("SecretInformation")
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder
                .Entity<Animal>()
                .HasOne(x => x.Employee)
                .WithMany(x => x.Animals)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull);
                

            // Настройка модели (таблицы в БД) сотрудников
            modelBuilder
                .Entity<Employee>()
                .ToTable("EmployeesOfZoo");

            modelBuilder
                .Entity<Employee>()
                .HasKey(X => X.Id);

            modelBuilder
                .Entity<Employee>()
                .Property(x => x.Name)
                .HasColumnName("Name");

            modelBuilder
                .Entity<Employee>()
                .Property(x => x.Position)
                .HasConversion<string>()
                .HasColumnName("Position");
        }
    }
}
