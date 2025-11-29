using ApplicationAnimal.Common.Abstractions.Animals;
using ApplicationAnimal.Common.ResultPattern;
using CSharpFunctionalExtensions;
using DomainAnimal.Entities;
using Infrastructure.ContextsDb;
using Microsoft.EntityFrameworkCore;

using Npgsql;
using System.Data.Common;

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

        public async Task<Animal?> GetAnimalByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var animal = await context.Animals.FindAsync(id);

            return animal;
        }

        public async Task<Result<string, Errors>> FeedAnimalAsync(int id, CancellationToken cancellationToken = default)
        {
            await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            {
                try
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

                catch(Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        public async Task<Result<bool, Errors>> DeleteAnimalAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleteAnimal = await context.Animals
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();

            if (deleteAnimal == 0)
                return GeneralErrors.NotFound().ToErrors();

            return true;
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
