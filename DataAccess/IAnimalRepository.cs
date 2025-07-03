using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IAnimalRepository
    {
        Task CreateAnimalAsync(Animal animal, CancellationToken cancellationToken = default);

        Task DeleteAnimalAsync(Animal animal, CancellationToken cancellationToken = default);
        Task<List<Animal>> GetAllAnimalsAsync(CancellationToken cancellationToken = default);
        Task<Animal> GetAnimalByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<string> FeedAnimalAsync(int id, CancellationToken cancellationToken = default);

    }
}
