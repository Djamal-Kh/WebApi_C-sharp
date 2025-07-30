using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataAccess;


namespace LibraryAnimals
{

    public class AnimalService(IAnimalRepository animalRepository) : IAnimalService
    {
        public async Task<Animal> CreateAnimalAsync(string TypeOfAnimal, string NameOfAnimal, CancellationToken cancellationToken = default)
        {
            Animal newAnimal;

            if (TypeOfAnimal == "Lion")
            {
                newAnimal = new Lion(NameOfAnimal);
                await animalRepository.CreateAnimalAsync(newAnimal);
            }
            else if (TypeOfAnimal == "Monkey")
            {
                newAnimal = new Monkey(NameOfAnimal);
                await animalRepository.CreateAnimalAsync(newAnimal);
            }           
            else
                throw new Exception("Type of Animal Not Found");

            return newAnimal;
        }

        public async Task<List<Animal>> GetAllAnimalsAsync(CancellationToken cancellationToken = default)
        {
            var animal = await animalRepository.GetAllAnimalsAsync();
            return animal;
        }

        public async Task<Animal> GetAnimalByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var animal = await animalRepository.GetAnimalByIdAsync(id);
            return animal;
        }

        public async Task<string> FeedAnimalAsync(int id, CancellationToken cancellationToken = default)
        {
            await Functions.CheckOnNull<IAnimal>(animalRepository, id);
            var result = await animalRepository.FeedAnimalAsync(id);
            return result;
        }

        public async Task<string> DeleteAnimalAsync(int id, CancellationToken cancellationToken = default)
        {
            await Functions.CheckOnNull<IAnimal>(animalRepository, id);
            var animal = await animalRepository.GetAnimalByIdAsync(id);

            await animalRepository.DeleteAnimalAsync(animal);
            string result = $"Животное с id = {id} было удалено";
            return result;
        }

    }
}
