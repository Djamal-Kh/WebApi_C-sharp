using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryAnimals
{
    public static class Extensions
    {
        public static IServiceCollection AddLibraryAnimals(this IServiceCollection services)
        {
            services.AddScoped<IAnimalService, AnimalService>();
            return services;
        }
    }
}
