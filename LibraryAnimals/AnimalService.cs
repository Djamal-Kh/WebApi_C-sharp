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
        public async Task<string> CreateAnimalAsync(string TypeOfAnimal, string NameOfAnimal, CancellationToken cancellationToken = default)
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
                throw new Exception("Вы ввели несуществующий тип животного в зоопарке. Если хотите добавить льва впишите \"Lion\", если обезьяну, то \"Monkey\" ");


            string json = JsonSerializer.Serialize(newAnimal);
            return json;
        }

        public async Task<string> DeleteAnimalAsync(int id, CancellationToken cancellationToken = default)
        {
            await Functions.CheckOnNull<IAnimal>(animalRepository, id);
            var animal = await animalRepository.GetAnimalByIdAsync(id);
            if (animal == null)
                throw new Exception("Животного с таким ID не существует");

            await animalRepository.DeleteAnimalAsync(animal);
            string json = JsonSerializer.Serialize(animal);
            return json;
        }

        public async Task<string> FeedAnimalAsync(int id, CancellationToken cancellationToken = default)
        {
            await Functions.CheckOnNull<IAnimal>(animalRepository, id);
            var animal = await animalRepository.GetAnimalByIdAsync(id);
            string json = JsonSerializer.Serialize(animal.Eat());
            return json;
        }

        public async Task<string> GetAllAnimalsAsync(CancellationToken cancellationToken = default)
        {
            await  Functions.CheckOnNull<IAnimal>(animalRepository);

            var animal = await animalRepository.GetAllAnimalsAsync(cancellationToken);
            string json = JsonSerializer.Serialize(animal);
            return json;
        }

        public async Task<string> GetAnimalByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            await Functions.CheckOnNull<IAnimal>(animalRepository, id);
            var animal = await animalRepository.GetAnimalByIdAsync(id);
            string json = JsonSerializer.Serialize(animal);
            return json;
        }
    }
}
