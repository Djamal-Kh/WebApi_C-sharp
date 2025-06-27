using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAnimals
{
    public interface IAnimalService
    {
        Task<string> GetAllAnimalsAsync(CancellationToken cancellationToken=default);
        Task<string> GetAnimalByIdAsync(int id, CancellationToken cancellationToken=default);
        Task<string> CreateAnimalAsync(string TypeOfAnimal, string NameOfAnimal, CancellationToken cancellationToken = default);
        Task<string> FeedAnimalAsync(int id, CancellationToken cancellationToken=default);
        Task<string> DeleteAnimalAsync(int id, CancellationToken cancellationToken = default);
    }
}
