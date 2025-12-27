using ApplicationAnimal.Common.DTO;
using ApplicationAnimal.Services.Caching;
using AutoMapper;
using CSharpFunctionalExtensions;
using DomainAnimal;
using DomainAnimal.Entities;
using DomainAnimal.Factories;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;
using ZooApi.DTO;

namespace ApplicationAnimal.Services.Animals
{

    public sealed class AnimalService : IAnimalService
    {
        private readonly ILogger<AnimalService> _logger;
        private readonly IAnimalRepository _animalRepository;
        private readonly HybridCache _cache;
        private readonly IMapper _mapper;

        public AnimalService(IAnimalRepository animalRepository, 
            ILogger<AnimalService> logger,
            HybridCache cache,
            IMapper mapper)
        {
            _logger = logger;
            _animalRepository = animalRepository;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<Result<AnimalResponseDto, Errors>> AddAnimalAsync(string type, string name, CancellationToken cancellationToken = default)
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

            var tags = new List<string> { AnimalConstants.ANIMAL_CACHE_TAG };

            await _cache.RemoveByTagAsync(tags, cancellationToken);

            var newAnimalResponse =  _mapper.Map<AnimalResponseDto>(newAnimal);

            return newAnimalResponse;
        }

        public async Task<Result<List<AnimalResponseDto>, Errors>> GetAllAnimalsAsync(CancellationToken cancellationToken = default)
        {
            string cacheKey = "animal:all_animals";

            var options = new HybridCacheEntryOptions
            {
                LocalCacheExpiration = TimeSpan.FromMinutes(1),
                Expiration = TimeSpan.FromMinutes(2)
            };

            var tags = new List<string> 
            { 
                AnimalConstants.ANIMAL_CACHE_TAG, 
                AnimalConstants.ANIMAL_COUNT_PREFIX 
            };

            var animals = await _cache.GetOrCreateAsync(
                cacheKey,
                async cancel =>
                {
                    _logger.LogInformation("Cache miss for key {CacheKey}. Retrieving from database.", cacheKey);

                    var animals = await _animalRepository.GetAllAnimalsAsync(cancel);
                    var animalsDto = _mapper.Map<List<AnimalResponseDto>>(animals);
                    return animalsDto;
                },
                options,
                tags,
                cancellationToken: cancellationToken
            );
            
            if (!animals.Any())
                return GeneralErrors.CollectionEmpty().ToErrors();

            return animals;
        }

        public async Task<Result<List<EnumAnimalTypeCountDto>, Errors>> GetNumberAnimalsByType(CancellationToken cancellationToken = default)
        {
            string cacheKey = "animal:animals_by_type";

            var options = new HybridCacheEntryOptions
            {
                LocalCacheExpiration = TimeSpan.FromMinutes(1),
                Expiration = TimeSpan.FromMinutes(2)
            };

            var tags = new List<string> { AnimalConstants.ANIMAL_COUNT_PREFIX };

            var animalsByType = await _cache.GetOrCreateAsync(
                cacheKey,
                async cancel =>
                {
                    _logger.LogInformation("Cache miss for key {CacheKey}. Retrieving from database.", cacheKey);

                    return await _animalRepository.GetNumberAnimalsByType(cancel);
                },
                options,
                tags,
                cancellationToken: cancellationToken
            );

            if (!animalsByType.Any())
            {
                return GeneralErrors.CollectionEmpty().ToErrors();
            }

            return animalsByType;
        }

        public async Task<Result<List<AnimalResponseDto>, Errors>> GetOwnerlessAnimals(CancellationToken cancellationToken = default)
        {
            string cacheKey = "animal:ownerless_animals";

            var options = new HybridCacheEntryOptions
            {
                LocalCacheExpiration = TimeSpan.FromMinutes(1),
                Expiration = TimeSpan.FromMinutes(2)
            };

            var tags = new List<string> 
            { 
                AnimalConstants.ANIMAL_CACHE_TAG, 
                AnimalConstants.ANIMAL_OWNERLESS_CACHE_TAG 
            };

            var ownerlessAnimals = await _cache.GetOrCreateAsync(
                cacheKey,
                async cancel =>
                {
                    _logger.LogInformation("Cache miss for key {CacheKey}. Retrieving from database.", cacheKey);

                    var animals = await _animalRepository.GetOwnerlessAnimals(cancel);
                    var animalsDto = _mapper.Map<List<AnimalResponseDto>>(animals);
                    return animalsDto;
                },
                options,
                tags,
                cancellationToken: cancellationToken
            );

            if (!ownerlessAnimals.Any())
            {
                return GeneralErrors.CollectionEmpty().ToErrors();
            }

            return ownerlessAnimals;
        }

        public async Task<Result<AnimalResponseDto, Errors>> GetAnimalByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            string cacheKey = $"animal:id:{id}";

            var options = new HybridCacheEntryOptions
            {
                LocalCacheExpiration = TimeSpan.FromMinutes(1),
                Expiration = TimeSpan.FromMinutes(2)
            };

            var tags = new List<string> 
            { 
                AnimalConstants.ANIMAL_BY_ID_CACHE_TAG + id,
                AnimalConstants.ALL_ANIMALS_BY_ID_CACHE_TAG
            };

            var animal = await _cache.GetOrCreateAsync(
                cacheKey,
                async cancel =>
                {
                    _logger.LogInformation("Cache miss for key {CacheKey}. Retrieving from database.", cacheKey);

                    var animal = await _animalRepository.GetAnimalByIdAsync(id, cancel);
                    var animalDto = _mapper.Map<AnimalResponseDto>(animal);
                    return animalDto;
                },
                options,
                tags,
                cancellationToken: cancellationToken
            );

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

            var tags = new List<string> 
            { 
                AnimalConstants.ANIMAL_CACHE_TAG, 
                AnimalConstants.ANIMAL_BY_ID_CACHE_TAG + id 
            };

            await _cache.RemoveByTagAsync(tags, cancellationToken);

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

            var tags = new List<string> 
            { 
                EmployeeConstants.EMPLOYEE_CACHE_TAG, 
                EmployeeConstants.EMPLOYEE_BY_ID_CACHE_TAG + animal.EmployeeId, 
                AnimalConstants.ANIMAL_CACHE_TAG, 
                AnimalConstants.ANIMAL_BY_ID_CACHE_TAG+id, 
                AnimalConstants.ANIMAL_COUNT_PREFIX 
            };

            await _cache.RemoveByTagAsync(tags, cancellationToken);

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

            int removedEmployeeId = await _animalRepository.RemoveBoundAnimalAsync(animal, cancellationToken);

            if(removedEmployeeId == -1)
            {
                return GeneralErrors.Failure("Failed to unbind employee from animal").ToErrors();
            }

            var tags = new List<string> 
            { 
                EmployeeConstants.EMPLOYEE_CACHE_TAG, 
                EmployeeConstants.EMPLOYEE_BY_ID_CACHE_TAG + animal.EmployeeId,
                AnimalConstants.ANIMAL_CACHE_TAG,
                AnimalConstants.ANIMAL_BY_ID_CACHE_TAG + id
            };
            
            await _cache.RemoveByTagAsync(tags, cancellationToken);

            return $"The employee was unbound from animal with id {id}";
        }

        public async Task<UnitResult<Errors>> DecrementAnimalEnergyAsync(int decrementValue, CancellationToken cancellationToken = default)
        {
            try
            {
                await _animalRepository.DecrementAnimalEnergyAsync(decrementValue, cancellationToken);

                return UnitResult.Success<Errors>();
            }
            catch
            {
                _logger.LogError("Error occurred while decrementing animal energy by {DecrementValue}", decrementValue);

                return GeneralErrors.Failure("Failed to decrement animal energy").ToErrors();
            }
        }
    }
}
