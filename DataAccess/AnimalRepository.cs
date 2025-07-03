using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AnimalRepository(AppContextDB context) : IAnimalRepository
    {
        public async Task CreateAnimalAsync(Animal animal, CancellationToken cancellationToken = default)
        {
            await context.Animals.AddAsync(animal, cancellationToken);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAnimalAsync(Animal animal, CancellationToken cancellationToken = default)
        {
           context.Animals.Remove(animal);
           await context.SaveChangesAsync();
        }

        public async Task<List<Animal>> GetAllAnimalsAsync(CancellationToken cancellationToken = default)
        {
            return await context.Animals.ToListAsync();
        }

        public async Task<Animal> GetAnimalByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await context.Animals.FirstOrDefaultAsync(x => x.Id == id);
        }
        
        public async Task<string> FeedAnimalAsync(int id, CancellationToken cancellationToken = default)
        {
            var WantedAnimal = await context.Animals.FindAsync(id);
            if (WantedAnimal == null)
            {
                throw new Exception("Животное не найдено");
            }

            var result = WantedAnimal.Eat();
            return result;
        }

    }
}
