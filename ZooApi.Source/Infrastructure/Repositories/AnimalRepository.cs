using ApplicationAnimal.Common.Interfaces;
using ApplicationAnimal.Common.ResultPattern;
using CSharpFunctionalExtensions;
using DomainAnimal.Entities;
using Infrastructure.ContextsDb;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public sealed class AnimalRepository(AppContextDB context) : IAnimalRepository
    {
        // REST
        public async Task CreateAnimalAsync(Animal animal, CancellationToken cancellationToken = default)
        {
            await context.Animals.AddAsync(animal);
            await context.SaveChangesAsync();
        }

        public async Task<List<Animal>> GetAllAnimalsAsync(CancellationToken cancellationToken = default)
        {
            return await context.Animals.OrderBy(a => a.Id).ToListAsync();
        }

        public async Task<Result<Animal, Errors>> GetAnimalByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var animal = await context.Animals.FindAsync(id);
            if (animal == null)
            {
                return GeneralErrors.NotFound(id).ToErrors();
            }
            return animal;
        }

        public async Task<string> FeedAnimalAsync(Animal animal, CancellationToken cancellationToken = default)
        {
            var feedingResult = animal.Eat();
            await context.SaveChangesAsync();
            return feedingResult;
        }

        public async Task DeleteAnimalAsync(Animal animal, CancellationToken cancellationToken = default)
        {
            context.Animals.Remove(animal);
            await context.SaveChangesAsync();
        }

        // Other methods (Not HTTP Methods)
        public async Task<bool> ExistsByName(string name, CancellationToken cancellationToken = default)
        {
            bool exist = await context.Animals.AnyAsync(n => n.Name == name);
            return exist;
        }

        public async Task DecrementAnimalEnergyAsync(int decrementValue, CancellationToken cancellationToken = default)
        {
            await context.Animals.Where(E => E.Energy > 0 && E.Energy >= decrementValue).ExecuteUpdateAsync(x => x.SetProperty(E => E.Energy, desE => desE.Energy - decrementValue));
        }
    }
}
