using ApplicationAnimal.Common.ResultPattern;
using DomainAnimal.Entities;

namespace ApplicationAnimal.Common.Interfaces
{
    public interface IAnimalService
    {
        Task<List<Animal>> GetAllAnimalsAsync(CancellationToken cancellationToken=default);
        Task<Animal> GetAnimalByIdAsync(int id, CancellationToken cancellationToken=default);
        Task<Result<Animal>> CreateAnimalAsync(AnimalType animalType, string NameOfAnimal, CancellationToken cancellationToken = default);
        Task<string> FeedAnimalAsync(int id, CancellationToken cancellationToken=default);
        Task<string> DeleteAnimalAsync(int id, CancellationToken cancellationToken = default);
    }
}
