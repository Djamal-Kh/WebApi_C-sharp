using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAnimals
{
    public interface IAnimalService
    {
        Task<List<Animal>> GetAllAnimalsAsync(CancellationToken cancellationToken=default);
        Task<Animal> GetAnimalByIdAsync(int id, CancellationToken cancellationToken=default);
        Task<Animal> CreateAnimalAsync(AnimalType animalType, string NameOfAnimal, CancellationToken cancellationToken = default);
        Task<string> FeedAnimalAsync(int id, CancellationToken cancellationToken=default);
        Task<string> DeleteAnimalAsync(int id, CancellationToken cancellationToken = default);
    }
}
