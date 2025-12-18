using ApplicationAnimal.Common.DTO;
using CSharpFunctionalExtensions;
using DomainAnimal.Entities;
using Shared.Common.ResultPattern;

namespace ApplicationAnimal.Services.Animals
{
    public interface IAnimalService
    {
        Task<Result<Animal, Errors>> AddAnimalAsync(string type, string NameOfAnimal, CancellationToken cancellationToken = default);
        Task<Result<List<Animal>, Errors>> GetAllAnimalsAsync(CancellationToken cancellationToken=default);
        Task<Result<List<EnumAnimalTypeCountDto>, Errors>> GetNumberAnimalsByType(CancellationToken cancellationToken = default);
        Task<Result<List<Animal>, Errors>> GetOwnerlessAnimals(CancellationToken cancellationToken = default);
        Task<Result<Animal, Errors>> GetAnimalByIdAsync(int id, CancellationToken cancellationToken=default);
        Task<Result<string, Errors>> FeedAnimalAsync(int id, CancellationToken cancellationToken=default);
        Task<Result<string, Errors>> DeleteAnimalAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<string, Errors>> RemoveBoundAnimalAsync(int id, CancellationToken cancellationToken = default);

    }
}
