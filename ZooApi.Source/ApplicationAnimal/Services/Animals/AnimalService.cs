using ApplicationAnimal.Common.DTO;
using CSharpFunctionalExtensions;
using DomainAnimal.Entities;
using DomainAnimal.Factories;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

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

        public async Task<Result<Animal, Errors>> AddAnimalAsync(string type, string name, CancellationToken cancellationToken = default)
        {
            bool isDuplicateName = await _animalRepository.isDuplicateNameAsync(name, cancellationToken);

            if (isDuplicateName)
            {
                _logger.LogWarning("Attempt to create an animal with Name {Name} is failed: Name duplication !", name);
                return GeneralErrors.ValueAlreadyExists(name).ToErrors();
            }

            EnumAnimalType enumType = (EnumAnimalType)Enum.Parse(
                typeof(EnumAnimalType), type, ignoreCase: true);

            Animal newAnimal = AnimalFactory.Create(enumType, name);

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

        public async Task<Result<List<EnumAnimalTypeCountDto>, Errors>> GetNumberAnimalsByType(CancellationToken cancellationToken = default)
        {
            var animalsByType = await _animalRepository.GetNumberAnimalsByType(cancellationToken);

            if (!animalsByType.Any())
            {
                return GeneralErrors.CollectionEmpty().ToErrors();
            }

            return animalsByType;
        }

        public async Task<Result<List<Animal>, Errors>> GetOwnerlessAnimals(CancellationToken cancellationToken = default)
        {
            var ownerlessAnimals = await _animalRepository.GetOwnerlessAnimals(cancellationToken);

            if (!ownerlessAnimals.Any())
            {
                return GeneralErrors.CollectionEmpty().ToErrors();
            }

            return ownerlessAnimals;
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

            return $"The animal with {id} was removed";
        }

        public async Task<Result<string, Errors>> RemoveBoundAnimalAsync(int id, CancellationToken cancellationToken = default)
        {
            var animal = await _animalRepository.GetAnimalByIdAsync(id, cancellationToken);

            if (animal is null)
            {
                return GeneralErrors.NotFound(id).ToErrors();
            }

            if (animal.EmployeeId is null)
            {
                return GeneralErrors.ValueIsInvalid($"No employee for animal with id {id}").ToErrors();
            }

            int removedEmoloyeeId = await _animalRepository.RemoveBoundAnimalAsync(animal, cancellationToken);

            if(removedEmoloyeeId == -1)
            {
                return GeneralErrors.Failure("Failed to unbind employee from animal").ToErrors();
            }

            return $"The employee was unbound from animal with id {id}";
        }
    }
}
