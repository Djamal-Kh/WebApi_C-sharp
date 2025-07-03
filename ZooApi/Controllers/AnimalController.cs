using Microsoft.AspNetCore.Mvc;
using LibraryAnimals;

namespace ZooApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController(IAnimalService animalService) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> CreateAnimal(string TypeOfAnimal, string NameOfAnimal)
        {
            try
            {
                var result = await animalService.CreateAnimalAsync(TypeOfAnimal, NameOfAnimal);
                return Ok(result);
            }

            catch (Exception)
            { return BadRequest(new { Message = "The animal species field is filled in incorrectly" }); }

        }


        [HttpGet]
        public async Task<IActionResult> GetAllAnimals()
        {
            var result = await animalService.GetAllAnimalsAsync();
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnimalById([FromRoute] int id)
        {
            try
            {
                var result = await animalService.GetAnimalByIdAsync(id);
                    return Ok(result);
            }

            catch { return NotFound(new { Message = "Not Found Animal" }); }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> FeedAnimal([FromRoute] int id)
        {
            try
            {
                var result = await animalService.FeedAnimalAsync(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest(new { Message = "Incorrect Data !" });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal([FromRoute]int id)
        {
            try
            {
                var result = await animalService.DeleteAnimalAsync(id);
                return NoContent();
            }
            catch
            {
                return NotFound(new { Message = "Not Found animal" });
            }
        }
    }
}
