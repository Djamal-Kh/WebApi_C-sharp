using ApplicationAnimal.Common.DTO;
using CSharpFunctionalExtensions;
using DomainAnimal.Entities;
using Shared.Common.ResultPattern;

namespace ApplicationAnimal.Services.Animals
{
    public interface IAnimalRepository
    {
        Task AddAnimalAsync(Animal animal, CancellationToken cancellationToken = default);
        Task<List<Animal>> GetAllAnimalsAsync(CancellationToken cancellationToken = default);
        Task<List<EnumAnimalTypeCountDto>> GetNumberAnimalsByType(CancellationToken cancellationToken = default);
        Task<List<Animal>> GetOwnerlessAnimals(CancellationToken cancellationToken = default);
        Task<Animal?> GetAnimalByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<string, Errors>> FeedAnimalAsync(int id, CancellationToken cancellationToken = default);
        Task DeleteAnimalAsync(Animal animal, CancellationToken cancellationToken = default);
        Task<int> RemoveBoundAnimalAsync(Animal animal, CancellationToken cancellationToken = default);
        Task<bool> isDuplicateNameAsync(string name, CancellationToken cancellationToken = default); 
        Task DecrementAnimalEnergyAsync(int decrementValue, CancellationToken cancellationToken = default);

    }
}
