using ApplicationAnimal.Common.Abstractions.Animals;
using ApplicationAnimal.Common.DTO;
using ApplicationAnimal.Common.ResultPattern;
using CSharpFunctionalExtensions;
using DomainAnimal.Entities;
using DomainAnimal.Factories;
using Microsoft.Extensions.Logging;

namespace ApplicationAnimal.Services.Animals
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

        public async Task<Result<Animal, Errors>> AddAnimalAsync(AnimalType animalType, string nameOfAnimal, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Try add Animal with Name: {Name}", nameOfAnimal);

            bool isDuplicateName = await _animalRepository.isDuplicateNameAsync(nameOfAnimal, cancellationToken);
            if (isDuplicateName)
            {
                _logger.LogWarning("Attempt to create an animal with Name: {Name} is failed !", nameOfAnimal);
                return GeneralErrors.ValueAlreadyExists(nameOfAnimal).ToErrors();
            }

            Animal newAnimal = AnimalFactory.Create(animalType, nameOfAnimal);
            await _animalRepository.AddAnimalAsync(newAnimal, cancellationToken);

            return newAnimal;
        }

        public async Task<Result<List<Animal>, Errors>> GetAllAnimalsAsync(CancellationToken cancellationToken = default)
        {
            var animals = await _animalRepository.GetAllAnimalsAsync(cancellationToken);

            if (!animals.Any())
                return GeneralErrors.CollectionEmpty().ToErrors();

            return animals;
        }

        public async Task<Result<Animal, Errors>> GetAnimalByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var animal = await _animalRepository.GetAnimalByIdAsync(id, cancellationToken);
            if (animal is null)
            {
                return GeneralErrors.NotFound(id).ToErrors();
            }

            return animal;
        }

        public Task<Result<List<AnimalTypeCountDto>, Errors>> GetNumberAnimalsByType(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<Animal>, Errors>> GetOwnerlessAnimals(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<string, Errors>> FeedAnimalAsync(int id, CancellationToken cancellationToken = default)
        {
            var feedingResult = await _animalRepository.FeedAnimalAsync(id, cancellationToken);

            if (feedingResult.IsFailure) 
                return GeneralErrors.NotFound(id).ToErrors();

            return feedingResult;
        }

        public async Task<Result<string, Errors>> DeleteAnimalAsync(int id, CancellationToken cancellationToken = default)
        {
            var animal = await _animalRepository.GetAnimalByIdAsync(id, cancellationToken);
            if (animal is null)
            {
                return GeneralErrors.NotFound(id).ToErrors();
            }

            await _animalRepository.DeleteAnimalAsync(animal, cancellationToken);

            return $"Животное с id = {id} было удалено";
        }
    }
}
