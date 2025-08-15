using DataAccess;


namespace LibraryAnimals
{

    public class AnimalService(IAnimalRepository animalRepository) : IAnimalService
    {
        public async Task<Animal> CreateAnimalAsync(AnimalType animalType, string NameOfAnimal, CancellationToken cancellationToken = default)
        {
            Animal newAnimal;

            if (animalType == AnimalType.Lion)
            {
                newAnimal = new Lion(NameOfAnimal);
                await animalRepository.CreateAnimalAsync(newAnimal);
            }
            else if (animalType == AnimalType.Monkey)
            {
                newAnimal = new Monkey(NameOfAnimal);
                await animalRepository.CreateAnimalAsync(newAnimal);
            }           
            else
                throw new ArgumentException();

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
            var result = await animalRepository.FeedAnimalAsync(id);
            return result;
        }

        public async Task<string> DeleteAnimalAsync(int id, CancellationToken cancellationToken = default)
        {
            var animal = await animalRepository.GetAnimalByIdAsync(id);
            await animalRepository.DeleteAnimalAsync(animal);
            string result = $"Животное с id = {id} было удалено";
            return result;
        }

    }
}
