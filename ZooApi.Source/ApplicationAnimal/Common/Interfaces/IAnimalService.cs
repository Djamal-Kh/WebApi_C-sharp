using ApplicationAnimal.Common.ResultPattern;
using DomainAnimal.Entities;
using CSharpFunctionalExtensions;

namespace ApplicationAnimal.Common.Interfaces
{
    public interface IAnimalService
    {
        Task<Result<List<Animal>, Errors>> GetAllAnimalsAsync(CancellationToken cancellationToken=default);
        Task<Result<Animal, Errors>> GetAnimalByIdAsync(int id, CancellationToken cancellationToken=default);
        Task<Result<Animal, Errors>> AddAnimalAsync(AnimalType animalType, string NameOfAnimal, CancellationToken cancellationToken = default);
        Task<Result<string, Errors>> FeedAnimalAsync(int id, CancellationToken cancellationToken=default);
        Task<Result<string, Errors>> DeleteAnimalAsync(int id, CancellationToken cancellationToken = default);
    }
}
