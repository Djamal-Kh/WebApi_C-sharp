using Microsoft.AspNetCore.Mvc;
using LibraryAnimals;
using AutoMapper;
using ZooApi.DTO;
using FluentValidation;

namespace ZooApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalService _animalService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAnimalDto> _createvAnimalValidator;

        public AnimalController(IAnimalService animalService, IMapper mapper, IValidator<CreateAnimalDto> createvAnimalValidator)   
        {
            _animalService = animalService;
            _mapper = mapper;
            _createvAnimalValidator = createvAnimalValidator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnimal([FromBody] CreateAnimalDto dto)
        {
            var validatorCreate = await _createvAnimalValidator.ValidateAsync(dto);
            if (!validatorCreate.IsValid)
            {
                var errorsValidation = validatorCreate.Errors.Select(x => new {x.AttemptedValue, x.ErrorMessage });
                return BadRequest(errorsValidation);
            }

            var animal = await _animalService.CreateAnimalAsync(dto.Type, dto.Name);
            var result = _mapper.Map<AnimalDto>(animal);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnimals()
        {
            var animals = await _animalService.GetAllAnimalsAsync();
            var result = _mapper.Map<List<AnimalDto>>(animals);
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnimalById([FromRoute] int id)
        {
            var animal = await _animalService.GetAnimalByIdAsync(id);
            var result = _mapper.Map<AnimalDto>(animal);
            return Ok(result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> FeedAnimal([FromRoute] int id)
        {
            var result = await _animalService.FeedAnimalAsync(id);
            return Ok(result);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal([FromRoute] int id)
        {
            var result = await _animalService.DeleteAnimalAsync(id);
            return Ok(result);
        }

    }
}
