using DomainAnimal.Entities;
using DomainAnimal.Interfaces;
using Infrastructure.ContextsDb;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;


namespace Infrastructure.Repositories
{
    public class AnimalRepository(AppContextDB context) : IAnimalRepository
    {
        // REST
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
            // Для того, чтобы в БД сохранить изменившуюся энергию у животного, которого накормили
            await context.SaveChangesAsync();
            return result;
        }


        public async Task DeleteAnimalAsync(Animal animal, CancellationToken cancellationToken = default)
        {
            context.Animals.Remove(animal);
            await context.SaveChangesAsync();
        }


        // Other (Not HTTP Methods)
        public async Task<bool> ExistsByName(string name, CancellationToken cancellationToken = default)
        {
            bool result = await context.Animals.AnyAsync(n => n.Name == name);
            return result;
        }


        public async Task DecrementAnimalEnergyAsync(int decrementValue, CancellationToken cancellationToken = default)
        {
            await context.Animals.Where(E => E.Energy > 0 && E.Energy >= decrementValue).ExecuteUpdateAsync(x => x.SetProperty(E => E.Energy, desE => desE.Energy - decrementValue));
        }
    }
}
