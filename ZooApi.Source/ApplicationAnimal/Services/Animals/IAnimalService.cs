using ApplicationAnimal.Common.DTO;
using CSharpFunctionalExtensions;
using DomainAnimal.Entities;
using Shared.Common.ResultPattern;
using ZooApi.DTO;

namespace ApplicationAnimal.Services.Animals
{
    public interface IAnimalService
    {
        Task<Result<AnimalResponseDto, Errors>> AddAnimalAsync(string type, string NameOfAnimal, CancellationToken cancellationToken = default);
        Task<Result<List<AnimalResponseDto>, Errors>> GetAllAnimalsAsync(CancellationToken cancellationToken=default);
        Task<Result<List<EnumAnimalTypeCountDto>, Errors>> GetNumberAnimalsByType(CancellationToken cancellationToken = default);
        Task<Result<List<AnimalResponseDto>, Errors>> GetOwnerlessAnimals(CancellationToken cancellationToken = default);
        Task<Result<AnimalResponseDto, Errors>> GetAnimalByIdAsync(int id, CancellationToken cancellationToken=default);
        Task<Result<string, Errors>> FeedAnimalAsync(int id, CancellationToken cancellationToken=default);
        Task<Result<string, Errors>> DeleteAnimalAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<string, Errors>> RemoveBoundAnimalAsync(int id, CancellationToken cancellationToken = default);
        Task<UnitResult<Errors>> DecrementAnimalEnergyAsync(int decrementValue, CancellationToken cancellationToken = default);
    }
}
