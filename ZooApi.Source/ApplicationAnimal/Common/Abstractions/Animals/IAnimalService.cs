using ApplicationAnimal.Common.DTO;
using ApplicationAnimal.Common.ResultPattern;
using CSharpFunctionalExtensions;
using DomainAnimal.Entities;

namespace ApplicationAnimal.Common.Abstractions.Animals
{
    public interface IAnimalService
    {
        Task<Result<List<Animal>, Errors>> GetAllAnimalsAsync(CancellationToken cancellationToken=default);
        Task<Result<Animal, Errors>> GetAnimalByIdAsync(int id, CancellationToken cancellationToken=default);
        Task<Result<List<AnimalTypeCountDto>, Errors>> GetNumberAnimalsByType(CancellationToken cancellationToken = default); // новое
        Task<Result<List<Animal>, Errors>> GetOwnerlessAnimals(CancellationToken cancellationToken = default); // новое
        Task<Result<Animal, Errors>> AddAnimalAsync(AnimalType animalType, string NameOfAnimal, CancellationToken cancellationToken = default);
        Task<Result<string, Errors>> FeedAnimalAsync(int id, CancellationToken cancellationToken=default);
        Task<Result<string, Errors>> DeleteAnimalAsync(int id, CancellationToken cancellationToken = default);
    }
}
