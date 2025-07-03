using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace DataAccess
{
    public class AppContextDB(DbContextOptions<AppContextDB> options) : DbContext(options)
    {
        public DbSet<Animal> Animals { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animal>()
                .HasDiscriminator<string>("AnimalType")
                .HasValue<Lion>("Lion")
                .HasValue<Monkey>("Monkey");

            base.OnModelCreating(modelBuilder);
        }
    }
}
