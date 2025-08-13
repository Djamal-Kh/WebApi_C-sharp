using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AnimalRepository(AppContextDB context) : IAnimalRepository
    {
        public async Task CreateAnimalAsync(Animal animal, CancellationToken cancellationToken = default)
        {
            await context.Animals.AddAsync(animal);
            await context.SaveChangesAsync();
        }

        public async Task<List<Animal>> GetAllAnimalsAsync(CancellationToken cancellationToken = default)
        {
            var result = await context.Animals.OrderBy(a => a.Id).ToListAsync();
            if (result.Count == 0)
                throw new SqlNullValueException();

            return result;
        }

        public async Task<Animal> GetAnimalByIdAsync(int id, CancellationToken cancellationToken = default)
        { 
            var result = await context.Animals.FindAsync(id);
            if (result == null)
                throw new KeyNotFoundException();
            return result;
        }
        
        public async Task<string> FeedAnimalAsync(int id, CancellationToken cancellationToken = default)
        {
            var TargetAnimal = await context.Animals.FindAsync(id);
            if (TargetAnimal == null)
            {
                throw new KeyNotFoundException();
            }
            var result = TargetAnimal.Eat();
            await context.SaveChangesAsync(); // Для того, чтобы в БД сохранить изменившуюся энергию у животного, которого накормили
            return result;
        }

        public async Task DeleteAnimalAsync(Animal animal, CancellationToken cancellationToken = default)
        {
            context.Animals.Remove(animal);
            await context.SaveChangesAsync();
        }

    }
}
