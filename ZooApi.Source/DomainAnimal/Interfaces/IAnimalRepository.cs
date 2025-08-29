using DomainAnimal.Entities;

namespace DomainAnimal.Interfaces
{
    public interface IAnimalRepository
    {
        Task CreateAnimalAsync(Animal animal, CancellationToken cancellationToken = default);
        Task DeleteAnimalAsync(Animal animal, CancellationToken cancellationToken = default);
        Task<List<Animal>> GetAllAnimalsAsync(CancellationToken cancellationToken = default);
        Task<Animal> GetAnimalByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<string> FeedAnimalAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistsByName(string name, CancellationToken cancellationToken = default);
        Task DecrementAnimalEnergyAsync(int decrementValue, CancellationToken cancellationToken = default);
    }
}
