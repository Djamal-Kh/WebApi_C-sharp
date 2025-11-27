using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ZooApi.DTO;
using FluentValidation;
using WebApiAnimal.Filters;
using WebApiAnimal.DTO;
using DomainAnimal.Entities;
using ApplicationAnimal.Common.Abstractions.Animals;

namespace ZooApi.Controllers
{
    [Route("api/[controller]")]
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
                "Start adding animal to the database. InputData: Type:{TypeOfAnimal} Name:{NameOfAnimal}",
                dto.Type, dto.Name);

            var createValidator = await _addAnimalValidator.ValidateAsync(dto);
            if (!createValidator.IsValid)
            {
                var validationErrors = createValidator.Errors
                    .Select(x => new ValidationErrorDto { AttemptedValue = x.AttemptedValue, ErrorMessage = x.ErrorMessage});

                _logger.LogWarning(
                    "Adding animal to the database failed. Validation is FAILED: {Validation}",
                    validationErrors);

                return BadRequest(validationErrors);
            }

            var result = await _animalService.AddAnimalAsync(dto.Type, dto.Name);
            if (result.IsFailure)
            {
                _logger.LogWarning(
                    "Adding animal to database failed. Incorrect data: {result}",
                    result);

                return BadRequest(result.Error);
            }

            Animal animal = result.Value;

            var animalDto = _mapper.Map<AddAnimalResponseDto>(animal);

            _logger.LogInformation(
                "Adding animal to database success. Property added animal: Id = {AnimalId}, Name = {AnimalName}, Type = {AnimalType}", 
                animalDto.Id, animalDto.Name, animalDto.Type);

            return Created(
                $"/api/animal/{animalDto.Id}",
                animalDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnimalsAsync()
        {
            _logger.LogInformation("Start GET. Return list of animals");

            var result = await _animalService.GetAllAnimalsAsync();

            List<Animal> animals = result.Value;
            var animalsDtos = _mapper.Map<List<AnimalResponseDto>>(animals);

            _logger.LogInformation("GET Success. Общее количество животных = {AnimalCount}",
                animals.Count);

            return Ok(animalsDtos);
        }

        [HttpGet("{animalId}")]
        [ServiceFilter(typeof(CacheAttribute))]
        public async Task<IActionResult> GetAnimalByIdAsync([FromRoute(Name = "animalId")] int id)
        {
            _logger.LogInformation("Start GET. Return the animal. InputData: Id = {AnimalId}", id);

            var result = await _animalService.GetAnimalByIdAsync(id);
            if (result.IsFailure) 
            {
                _logger.LogWarning("Get animal failed. Not found animal with id {Id}" , id);
                return NotFound(result.Error);
            } 

            Animal animal = result.Value;    
            var animalDto = _mapper.Map<AnimalResponseDto>(animal);

            _logger.LogInformation("Get animal Success. Property found Animal: " +
                "Id = {AnimalId}, Name = {AnimalName}, Type = {AnimalType}",
                animalDto.Id, animalDto.Name, animalDto.Type);

            return Ok(animalDto);
        }

        [HttpGet]
        Task<List<Animal>> GetNumberAnimalsByType(CancellationToken cancellationToken = default) // новое
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        Task<List<Animal>> GetOwnerlessAnimals(CancellationToken cancellationToken = default) // новое
        {
            throw new NotImplementedException();
        }

        [HttpPatch("{animalId}")]
        public async Task<IActionResult> FeedAnimalAsync([FromRoute(Name = "animalId")] int id)
        {
            _logger.LogInformation("Start Patch - feeding the animal. InputData: Id = {AnimalId}", id);

            var result = await _animalService.FeedAnimalAsync(id);
            if (result.IsFailure)
            {
                _logger.LogWarning("Patch animal failed. Not found animal with id {Id}", id);
                return NotFound(result.Error);
            }

            string feedingMessage = result.Value;

            _logger.LogInformation("PATCH (feeding Animal) success with id {Id} !", id);
            return Ok(feedingMessage);
        }

        [HttpDelete("{animalId}")]
        public async Task<IActionResult> DeleteAnimalAsync([FromRoute(Name = "animalId")] int id)
        {
            _logger.LogInformation("Start Delete animal. InputData: Id = {AnimalId}", id);

            var result = await _animalService.DeleteAnimalAsync(id);

            if (result.IsFailure)
            {
                _logger.LogWarning("Delete animal failed. Not found animal with id {Id}" , id);
                return NotFound(result.Error);
            }

           string deleteMessage = result.Value;

            _logger.LogInformation("Delete animal success with Id = {AnimalId}", id);
            return Ok(deleteMessage);
        }
    }
}
