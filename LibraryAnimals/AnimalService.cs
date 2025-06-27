using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static LibraryAnimals.IAnimal;


namespace LibraryAnimals
{

    public class AnimalService(List<IAnimal> animals) : IAnimalService
    {
        public async Task<string> CreateAnimalAsync(string TypeOfAnimal, string NameOfAnimal, CancellationToken cancellationToken = default)
        {
            IAnimal newAnimal;

            if (TypeOfAnimal == "Lion")
            {
                newAnimal = new Lion(NameOfAnimal);
                animals.Add(newAnimal);
            }
            else if (TypeOfAnimal == "Monkey")
            {
                newAnimal = new Monkey(NameOfAnimal);
                animals.Add(newAnimal);
            }           
            else
                throw new Exception("Вы ввели несуществующий тип животного в зоопарке. Если хотите добавить льва впишите \"Lion\", если обезьяну, то \"Monkey\" ");


            string json = JsonSerializer.Serialize(newAnimal);
            await Task.Yield();
            return json;
        }

        public async Task<string> DeleteAnimalAsync(int id, CancellationToken cancellationToken = default)
        {
            --id;
            await Functions.CheckOnNull<IAnimal>(animals, id);
            string json = JsonSerializer.Serialize(animals.FirstOrDefault(a => a.Id == id));
            animals.RemoveAll(a => a.Id == id);
            return json;
        }

        public async Task<string> FeedAnimalAsync(int id, CancellationToken cancellationToken = default)
        {
            --id;
            await Functions.CheckOnNull<IAnimal>(animals, id);
            string json = JsonSerializer.Serialize(animals[id].Eat());
            return json;
        }

        public async Task<string> GetAllAnimalsAsync(CancellationToken cancellationToken = default)
        {
            bool check = await Functions.CheckOnNull<IAnimal>(animals);

            string json = JsonSerializer.Serialize(animals);
            return json;
        }

        public async Task<string> GetAnimalByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            bool check = await Functions.CheckOnNull<IAnimal>(animals, id);
            string json = JsonSerializer.Serialize(animals.FirstOrDefault(a => a.Id == id));
            return json;
        }
    }
}
