using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ZooApi.DTO;
using FluentValidation;
using DomainAnimal.Interfaces;
using WebApiAnimal.Filters;

namespace ZooApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalService _animalService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAnimalDto> _createvAnimalValidator;
        private readonly ILogger<AnimalController> _logger;

        public AnimalController(ILogger<AnimalController> logger, IAnimalService animalService, IMapper mapper, IValidator<CreateAnimalDto> createvAnimalValidator)
        {
            _animalService = animalService;
            _mapper = mapper;
            _createvAnimalValidator = createvAnimalValidator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnimal([FromBody] CreateAnimalDto dto)
        {
            _logger.LogInformation("Запуск метода Http POST для создания нового животного. Входные данные: Type:{TypeOfAnimal} Name:{NameOfAnimal}", dto.Type, dto.Name);
            var validatorCreate = await _createvAnimalValidator.ValidateAsync(dto);
            if (!validatorCreate.IsValid)
            {
                var errorsValidation = validatorCreate.Errors.Select(x => new { x.AttemptedValue, x.ErrorMessage });
                _logger.LogWarning("Входные данные не прошли валидацию: {Validation}", errorsValidation);
                return BadRequest(errorsValidation);
            }

            var animal = await _animalService.CreateAnimalAsync(dto.Type, dto.Name);
            var result = _mapper.Map<AnimalDto>(animal);
            _logger.LogInformation("Успешно создано новое животное с Id = {AnimalId}, Name = {AnimalName}, Type = {AnimalType}", result.Id, result.Name, result.Type);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnimals()
        {
            _logger.LogInformation("Запуск метода Http GET для вывода информации о всех животных");
            var animals = await _animalService.GetAllAnimalsAsync();
            var result = _mapper.Map<List<AnimalDto>>(animals);
            _logger.LogInformation("Успешный вывод всех животных содержащихся в БД. Общее количество животных = {AnimalCount}", animals.Count);
            return Ok(result);
        }


        [HttpGet("{id}")]
        [ServiceFilter(typeof(CacheAttribute))]
        public async Task<IActionResult> GetAnimalById([FromRoute] int id)
        {
            _logger.LogInformation("Запуск метода Http GET для вывода информации о животном с Id = {AnimalId}", id);
            var animal = await _animalService.GetAnimalByIdAsync(id);
            var result = _mapper.Map<AnimalDto>(animal);
            _logger.LogInformation("Успешный вывод животного с Id = {AnimalId}, Name = {AnimalName}, Type = {AnimalType}", result.Id, result.Name, result.Type);
            return Ok(result);
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> FeedAnimal([FromRoute] int id)
        {
            _logger.LogInformation("Запуск метода Http PATCH для кормления животного с Id = {AnimalId}", id);
            var result = await _animalService.FeedAnimalAsync(id);
            _logger.LogInformation("Метод PATCH (кормление) прошел успешно !");
            return Ok(result);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal([FromRoute] int id)
        {
            _logger.LogInformation("Запуск метода Http Delete для удаления животного с Id = {AnimalId}", id);
            var result = await _animalService.DeleteAnimalAsync(id);
            _logger.LogInformation("Успешное удаление животного с Id = {AnimalId}", id);
            return Ok(result);
        }

    }
}
