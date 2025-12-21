using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ZooApi.DTO;
using FluentValidation;
using WebApiAnimal.Filters;
using WebApiAnimal.DTO;
using DomainAnimal.Entities;
using ApplicationAnimal.Services.Animals;
using ApplicationAnimal.Common.DTO;
using ApplicationAnimal.DTO;

namespace ZooApi.Controllers
{
    [Route("api/animal")]
    [ApiController]
    public sealed class AnimalController : ControllerBase
    {
        private readonly IAnimalService _animalService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddAnimalRequestDto> _addAnimalValidator;
        private readonly ILogger<AnimalController> _logger;

        public AnimalController(ILogger<AnimalController> logger, IAnimalService animalService, IMapper mapper, IValidator<AddAnimalRequestDto> createvAnimalValidator)
        {
            _animalService = animalService;
            _mapper = mapper;
            _addAnimalValidator = createvAnimalValidator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddAnimalAsync([FromBody] AddAnimalRequestDto dto)
        {
            _logger.LogInformation(
                "Try to add animal to the zoo. InputData: Type:{Type} Name:{Name}",
                dto.Type, dto.Name);

            var validation = await _addAnimalValidator.ValidateAsync(dto);
            if (!validation.IsValid)
            {
                var validationErrors = validation.Errors;

                _logger.LogWarning(
                    "Adding thr animal cancelled. Validation is FAILED: {Validation}",
                    validationErrors);

                return BadRequest(validationErrors);
            }

            var result = await _animalService.AddAnimalAsync(dto.Type, dto.Name);

            if (result.IsFailure)
            {
                _logger.LogWarning(
                    "Adding the animal cancelled. Incorrect data: {result}",
                    result);

                return BadRequest(result.Error);
            }

            Animal createdAnimal = result.Value;

            var responseDto = _mapper.Map<AddAnimalResponseDto>(createdAnimal);

            _logger.LogInformation(
                "Adding the animal successfully. Property added animal: Id = {Id}, Name = {Name}, Type = {Type}",
                responseDto.Id, responseDto.Name, responseDto.Type);

            return Created(
                $"/api/animal/{responseDto.Id}",
                responseDto);
        }

        [HttpGet]
        [TypeFilter(typeof(CacheAttributeForHttpGet))]
        public async Task<IActionResult> GetAllAnimalsAsync()
        {
            _logger.LogInformation("Return list of animals");

            var result = await _animalService.GetAllAnimalsAsync();

            if (result.IsFailure)
                return Ok(result.Error);

            List<Animal> animals = result.Value;

            var responseDto = _mapper.Map<List<AnimalResponseDto>>(animals);

            _logger.LogInformation("Total number of animals = {Count}",
                animals.Count);

            return Ok(responseDto);
        }

        [HttpGet]
        [Route("animal-type")]
        [TypeFilter(typeof(CacheAttributeForHttpGet))]
        public async Task<IActionResult> GetNumberAnimalsByType(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Return number of animals by type");

            var result = await _animalService.GetNumberAnimalsByType();

            if (result.IsFailure)
                return Ok(result.Error);

            var responseDto = _mapper.Map<List<GetNumberAnimalsTypeResponseDto>>(result.Value);

            return Ok(responseDto);
        }

        [HttpGet]
        [Route("ownerless-animals")]
        [TypeFilter(typeof(CacheAttributeForHttpGet))]
        public async Task<IActionResult> GetOwnerlessAnimals(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Return ownerless animals");

            var result = await _animalService.GetOwnerlessAnimals();

            if (result.IsFailure)
                return Ok(result.Error);

            return Ok(result.Value);
        }

        [HttpGet("{animalId}")]
        [TypeFilter(typeof(CacheAttributeForHttpGet))]
        public async Task<IActionResult> GetAnimalByIdAsync([FromRoute(Name = "animalId")] int id)
        {
            _logger.LogInformation("Try to return the animal. InputData: Id = {Id}", id);

            var result = await _animalService.GetAnimalByIdAsync(id);
            if (result.IsFailure) 
            {
                _logger.LogWarning("Not found animal with id {Id}" , id);
                return NotFound(result.Error);
            } 

            Animal animal = result.Value;    

            var responseDto = _mapper.Map<AnimalResponseDto>(animal);

            _logger.LogInformation("Property founded animal: " +
                "Id = {Id}, Name = {Name}, Type = {Type}",
                responseDto.Id, responseDto.Name, responseDto.Type);

            return Ok(responseDto);
        }

        [HttpPatch("{animalId}")]
        public async Task<IActionResult> FeedAnimalAsync([FromRoute(Name = "animalId")] int id)
        {
            _logger.LogInformation("Try to feed the animal. InputData: Id = {Id}", id);

            var result = await _animalService.FeedAnimalAsync(id);
            if (result.IsFailure)
            {
                _logger.LogWarning("Feeding cancelled. Not found animal with id {Id}", id);
                return NotFound(result.Error);
            }

            string feedingMessage = result.Value;

            _logger.LogInformation("Feeding the animal successfully. Id = {Id}", id);
            return Ok(feedingMessage);
        }

        [HttpDelete("{animalId}")]
        public async Task<IActionResult> DeleteAnimalAsync([FromRoute(Name = "animalId")] int id)
        {
            _logger.LogInformation("Try to remove animal. InputData: Id = {Id}", id);

            var result = await _animalService.DeleteAnimalAsync(id);

            if (result.IsFailure)
            {
                _logger.LogWarning("Removing the animal cancelled. Not found animal with id {Id}" , id);
                return NotFound(result.Error);
            }

           string deleteMessage = result.Value;

            _logger.LogInformation("Removing the animal successfully. Id = {Id}", id);
            return Ok(deleteMessage);
        }
    }
}
