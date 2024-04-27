using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Cw3.Animal;


namespace Cw3.Animal
{
    [Route("api/animals")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalRepository _animalRepository;

        public AnimalsController(IAnimalRepository animalRepository)
        {
            _animalRepository = animalRepository ?? throw new ArgumentNullException(nameof(animalRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnimals(string orderBy = "name")
        {
            var animals = await _animalRepository.GetAnimals(orderBy);
            return Ok(animals);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnimal([FromBody] PostAnimal postAnimal)
        {
            if (await _animalRepository.AnimalExists(postAnimal.Id))
            {
                return Conflict(new { message = "An animal with the given ID already exists." });
            }

            var animal = new Animal
            {
                Id = postAnimal.Id,
                Name = postAnimal.Name,
                Description = postAnimal.Description,
                Category = postAnimal.Category,
                Area = postAnimal.Area
            };
            
            await _animalRepository.AddAnimal(animal);

            return CreatedAtAction(nameof(GetAllAnimals), new { id = animal.Id }, postAnimal);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAnimal(string id, PutAdnimal putAdnimal)
        {
            if (!(await _animalRepository.AnimalExists(int.Parse(id))))
            {
                return NotFound(new { message = $"Animal with ID {id} not found." });
            }

            await _animalRepository.UpdateAnimal(int.Parse(id),putAdnimal);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeteleAnimal(string id)
        {
            if (!await _animalRepository.AnimalExists(int.Parse(id)))
            {
                return NotFound(new { message = $"Animal with ID {id} not found." });
            }

            await _animalRepository.DeleteAnimal(int.Parse(id));

            return Ok();

        }
    }
}
