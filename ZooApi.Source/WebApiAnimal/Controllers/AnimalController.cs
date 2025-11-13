using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ZooApi.DTO;
using FluentValidation;
using WebApiAnimal.Filters;
using WebApiAnimal.DTO;
using ApplicationAnimal.Common.Interfaces;
using DomainAnimal.Entities;

namespace ZooApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class AnimalController : ControllerBase
    {
        private readonly IAnimalService _animalService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAnimalDto> _createAnimalValidator;
        private readonly ILogger<AnimalController> _logger;

        public AnimalController(ILogger<AnimalController> logger, IAnimalService animalService, IMapper mapper, IValidator<CreateAnimalDto> createvAnimalValidator)
        {
            _animalService = animalService;
            _mapper = mapper;
            _createAnimalValidator = createvAnimalValidator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnimalAsync([FromBody] CreateAnimalDto dto)
        {
            _logger.LogInformation("Start Http POST (Animal). InputData: Type:{TypeOfAnimal} Name:{NameOfAnimal}", dto.Type, dto.Name);

            var validatorCreate = await _createAnimalValidator.ValidateAsync(dto);
            if (!validatorCreate.IsValid)
            {
                var validationErrors = validatorCreate.Errors
                    .Select(x => new ValidationErrorDto { AttemptedValue = x.AttemptedValue, ErrorMessage = x.ErrorMessage});

                _logger.LogWarning("Http POST (Animal) failed. Validation is FAILED: {Validation}", validationErrors);
                return BadRequest(validationErrors);
            }

            var result = await _animalService.CreateAnimalAsync(dto.Type, dto.Name);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            Animal createdAnimal = result.Value;
            var createdAnimalDto = _mapper.Map<AnimalDto>(createdAnimal);

            _logger.LogInformation("Http POST (Animal) success. Property added object: Id = {AnimalId}, Name = {AnimalName}, Type = {AnimalType}", 
                createdAnimalDto.Id, createdAnimalDto.Name, createdAnimalDto.Type);

            return Created(
                $"/api/animal/{createdAnimalDto.Id}",
                createdAnimalDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnimalsAsync()
        {
            _logger.LogInformation("Start Http GET (return list animals)");

            var result = await _animalService.GetAllAnimalsAsync();

            List<Animal> animals = result.Value;
            var animalsDtos = _mapper.Map<List<AnimalDto>>(animals);

            _logger.LogInformation("GET (Animals) Success. Общее количество животных = {AnimalCount}",
                animals.Count);
            return Ok(animalsDtos);
        }

        [HttpGet("{animalId}")]
        [ServiceFilter(typeof(CacheAttribute))]
        public async Task<IActionResult> GetAnimalByIdAsync([FromRoute(Name = "animalId")] int id)
        {
            _logger.LogInformation("Start Http GET (return Animal). InputData: Id = {AnimalId}", id);

            var result = await _animalService.GetAnimalByIdAsync(id);
            if (result.IsFailure) 
            {
                return NotFound(result.Error);
            } 

            Animal animal = result.Value;    
            var animalDto = _mapper.Map<AnimalDto>(animal);

            _logger.LogInformation("Http Get (Animal) Success. Property found Animal: Id = {AnimalId}, Name = {AnimalName}, Type = {AnimalType}",
                animalDto.Id, animalDto.Name, animalDto.Type);
            return Ok(animalDto);
        }

        [HttpPatch("{animalId}")]
        public async Task<IActionResult> FeedAnimalAsync([FromRoute(Name = "animalId")] int id)
        {
            _logger.LogInformation("Http PATCH (feed Animal). InputData: Id = {AnimalId}", id);

            var result = await _animalService.FeedAnimalAsync(id);
            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            string feedingMessage = result.Value;

            _logger.LogInformation("Http PATCH (feeding Animal) success !");
            return Ok(feedingMessage);
        }

        [HttpDelete("{animalId}")]
        public async Task<IActionResult> DeleteAnimalAsync([FromRoute(Name = "animalId")] int id)
        {
            _logger.LogInformation("Http Delete (Animal). InputData: Id = {AnimalId}", id);

            var result = await _animalService.DeleteAnimalAsync(id);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

           string deleteMessage = result.Value;

            _logger.LogInformation("Http Delete (Animal) success with Id = {AnimalId}", id);
            return Ok(deleteMessage);
        }
    }
}
