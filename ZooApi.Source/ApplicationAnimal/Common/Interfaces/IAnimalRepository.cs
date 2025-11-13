using ApplicationAnimal.Common.ResultPattern;
using CSharpFunctionalExtensions;
using DomainAnimal.Entities;

namespace ApplicationAnimal.Common.Interfaces
{
    public interface IAnimalRepository
    {
        Task CreateAnimalAsync(Animal animal, CancellationToken cancellationToken = default);
        Task DeleteAnimalAsync(Animal animal, CancellationToken cancellationToken = default);
        Task<List<Animal>> GetAllAnimalsAsync(CancellationToken cancellationToken = default);
        Task<Result<Animal, Errors>> GetAnimalByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<string> FeedAnimalAsync(Animal animal, CancellationToken cancellationToken = default);
        Task<bool> ExistsByName(string name, CancellationToken cancellationToken = default);
        Task DecrementAnimalEnergyAsync(int decrementValue, CancellationToken cancellationToken = default);
    }
}
