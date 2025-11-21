using ApplicationAnimal.Common.Interfaces;
using ApplicationAnimal.Common.ResultPattern;
using CSharpFunctionalExtensions;
using DomainAnimal.Entities;
using Infrastructure.ContextsDb;
using Microsoft.EntityFrameworkCore;

using Npgsql;

namespace Infrastructure.Repositories
{
    public sealed class AnimalRepository(AppContextDB context) : IAnimalRepository
    {
        public async Task AddAnimalAsync(Animal animal, CancellationToken cancellationToken = default)
        {
            await context.Animals.AddAsync(animal);
            await context.SaveChangesAsync();
        }

        public async Task<List<Animal>> GetAllAnimalsAsync(CancellationToken cancellationToken = default)
        {
            return await context.Animals
                .AsNoTracking()
                .OrderBy(a => a.Id)
                .ToListAsync();
        }

        public async Task<Result<Animal, Errors>> GetAnimalByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var animal = await context.Animals.FindAsync(id);

            if (animal is null)
                return GeneralErrors.NotFound(id).ToErrors();

            return animal;
        }

        public async Task<Result<string, Errors>> FeedAnimalAsync(int id, CancellationToken cancellationToken = default)
        {
            await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            {
                var animal = await context.Animals
                    .FromSqlRaw(
                    "Select * FROM \"AnimalsOfZoo\" WHERE \"Id\" = {0} FOR NO KEY UPDATE", id
                    )
                    .AsTracking()
                    .FirstOrDefaultAsync(cancellationToken);

                if (animal is null)
                    return GeneralErrors.NotFound(id).ToErrors();

                string feedingResult = animal.Eat();

                await context.SaveChangesAsync();

                await transaction.CommitAsync();

                return feedingResult;
            }
        }

        public async Task DeleteAnimalAsync(Animal animal, CancellationToken cancellationToken = default)
        {
            context.Animals.Remove(animal);
            await context.SaveChangesAsync();
        }



        public async Task<bool> isDuplicateNameAsync(string name, CancellationToken cancellationToken = default)
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
