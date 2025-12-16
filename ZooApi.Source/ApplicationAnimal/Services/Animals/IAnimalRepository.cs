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
        Task<Animal?> GetAnimalByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<AnimalTypeCountDto>> GetNumberAnimalsByType(CancellationToken cancellationToken = default); // новое
        Task<List<Animal>> GetOwnerlessAnimals(CancellationToken cancellationToken = default); // новое
        Task<Result<string, Errors>> FeedAnimalAsync(int id, CancellationToken cancellationToken = default); // передалть по ТЗ
        Task DeleteAnimalAsync(Animal animal, CancellationToken cancellationToken = default);
        // мб унифицировать метод для Animal и Employee
        Task<bool> isDuplicateNameAsync(string name, CancellationToken cancellationToken = default); 
        Task DecrementAnimalEnergyAsync(int decrementValue, CancellationToken cancellationToken = default);
        Task RemoveBoundAnimalAsync(int animalId, CancellationToken cancellationToken = default); 
    }
}
