using ApplicationAnimal.Common.DTO;
using ApplicationAnimal.Services.Animals;
using CSharpFunctionalExtensions;
using DomainAnimal.Entities;
using DomainAnimal.Interfaces;
using Infrastructure.ContextsDb;
using Microsoft.EntityFrameworkCore;

using Npgsql;
using Shared.Common.ResultPattern;

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

        public async Task<List<EnumAnimalTypeCountDto>> GetNumberAnimalsByType(CancellationToken cancellationToken = default)
        {
            return await context.Animals
                .GroupBy(t => t.Type)
                .Select(a => new EnumAnimalTypeCountDto(a.Key, a.Count()))
                .ToListAsync();
        }

        public async Task<List<Animal>> GetOwnerlessAnimals(CancellationToken cancellationToken = default)
        {
            var ownerlessAnimals = await context.Animals.Where(ei => ei.EmployeeId == null).ToListAsync();
            return ownerlessAnimals;
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
                    "Select * FROM animals WHERE id = {0} FOR NO KEY UPDATE"
                    , id
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

        public async Task DeleteAnimalAsync(Animal animal, CancellationToken cancellationToken = default)
        {
            context.Animals.Remove(animal);
            await context.SaveChangesAsync();
        }

        public async Task<int> RemoveBoundAnimalAsync(Animal animal, CancellationToken cancellationToken = default)
        {
            int? removedEmployeeId = animal.EmployeeId;

            await context.Animals
                .Where(a => a.Id == animal.Id)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(a => a.EmployeeId, desE => null));

            return removedEmployeeId ?? -1;
        }

        public async Task<bool> isDuplicateNameAsync(string name, CancellationToken cancellationToken = default)
        {
            bool exist = await context.Animals.AnyAsync(n => n.Name == name);

            return exist;
        }

        public async Task DecrementAnimalEnergyAsync(int decrementValue, CancellationToken cancellationToken = default)
        {
            await context.Animals.Where(E => E.Energy > 0 && E.Energy >= decrementValue)
                .ExecuteUpdateAsync(x => 
                    x.SetProperty(E => E.Energy, desE => desE.Energy - decrementValue));
        }
    }
}
