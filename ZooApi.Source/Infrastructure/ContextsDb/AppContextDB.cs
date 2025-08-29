using DomainAnimal.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.ContextsDb
{
    public class AppContextDB : DbContext
    {
        public AppContextDB(DbContextOptions<AppContextDB> options) : base(options) { }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Lion> Lions { get; set; }
        public DbSet<Monkey> Monkeys { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
                .Property(x => x.SecretInformation)
                .HasColumnName("SecretInformation")
                .HasDefaultValueSql("gen_random_uuid()");
        }
    }
}
