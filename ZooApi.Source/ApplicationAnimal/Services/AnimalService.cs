using ApplicationAnimal.Common.Interfaces;
using ApplicationAnimal.Common.ResultPattern;
using DomainAnimal.Entities;
using DomainAnimal.Factories;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace ApplicationAnimal.Services
{

    public sealed class AnimalService : IAnimalService
    {
        private readonly ILogger<AnimalService> _logger;
        private readonly IAnimalRepository _animalRepository;

        public AnimalService(IAnimalRepository animalRepository, ILogger<AnimalService> logger)
        {
            _logger = logger;
            _animalRepository = animalRepository;
        }

        public async Task<Result<Animal>> CreateAnimalAsync(AnimalType animalType, string nameOfAnimal, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Try add Animal with Name: {Name}", nameOfAnimal);
            bool existsName = await _animalRepository.ExistsByName(nameOfAnimal, cancellationToken);
            if (existsName)
            {
                _logger.LogWarning("Attempt to create an animal with Name: {Name} is failed !", nameOfAnimal);
                return Result<Animal>.Failure(new Error("Already exists with this name", "BadRequest"));
            }

            Animal newAnimal = AnimalFactory.Create(animalType, nameOfAnimal);
            await _animalRepository.CreateAnimalAsync(newAnimal, cancellationToken);
            return Result<Animal>.Success(newAnimal);
        }

        public async Task<List<Animal>> GetAllAnimalsAsync(CancellationToken cancellationToken = default)
        {
            var animals = await _animalRepository.GetAllAnimalsAsync(cancellationToken);
            if(!animals.Any())
            {
                _logger.LogWarning("No animals in the Database !");
                throw new SqlNullValueException();
            }


            return animals;
        }

        public async Task<Animal> GetAnimalByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _animalRepository.GetAnimalByIdAsync(id, cancellationToken);
        }

        public async Task<string> FeedAnimalAsync(int id, CancellationToken cancellationToken = default)
        {
            var animal = await _animalRepository.GetAnimalByIdAsync(id, cancellationToken);
            var feedingResult = await _animalRepository.FeedAnimalAsync(animal, cancellationToken);
            return feedingResult;
        }

        public async Task<string> DeleteAnimalAsync(int id, CancellationToken cancellationToken = default)
        {
            var animal = await _animalRepository.GetAnimalByIdAsync(id, cancellationToken);
            await _animalRepository.DeleteAnimalAsync(animal, cancellationToken);
            return $"Животное с id = {id} было удалено";
        }
    }
}
