using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccess
{
    public class AppContextDBFactory : IDesignTimeDbContextFactory<AppContextDB>
    {
        public AppContextDB CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppContextDB>();
            optionsBuilder.UseNpgsql("Host=localhost;Database=ZooDb;Username=postgres;Password=Su132435wwwq");
            return new AppContextDB(optionsBuilder.Options);
        }
    }
}

