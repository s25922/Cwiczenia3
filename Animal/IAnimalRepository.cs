using System.Data.SqlClient;

namespace Cw3.Animal
{
    public interface IAnimalRepository
    {
        public Task<List<Animal>> GetAnimals(string orderBy);
        public Task AddAnimal(Animal animal);
        public Task UpdateAnimal(int id, PutAdnimal putAdnimal);
        public Task DeleteAnimal(int id);
        public Task<bool> AnimalExists(int id);
        
    }
}