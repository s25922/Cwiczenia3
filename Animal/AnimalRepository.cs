using System.Data.SqlClient;

namespace Cw3.Animal
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly string _connectionString;
        public AnimalRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }
        

        public async Task<List<Animal>> GetAnimals(string orderBy)
        {
            var animals = new List<Animal>();
            var query = "SELECT Id, Name, Description, Category, Area FROM Animals";
            
            var validSortColumns = new HashSet<string> { "Id","Name", "Description", "Category", "Area", "id", "name", "description", "category", "area"};
            if (!string.IsNullOrEmpty(orderBy) && validSortColumns.Contains(orderBy))
            {
                query += $" ORDER BY {orderBy}";
            }
            else if (!string.IsNullOrEmpty(orderBy))
            {
                throw new ArgumentException("Invalid column name for sorting.");
            }
            
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var animal = new Animal
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                            Category = reader.GetString(reader.GetOrdinal("Category")),
                            Area = reader.GetString(reader.GetOrdinal("Area"))
                        };
                        animals.Add(animal);
                    }
                }

            }
            return animals;
        }
        public async Task AddAnimal(Animal animal)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("Insert into Animals (Id, Name, Description, Category, Area) values (@Id, @Name, @Description, @Category, @Area)", connection);
                command.Parameters.AddWithValue("@Id", animal.Id);
                command.Parameters.AddWithValue("@Name", animal.Name);
                command.Parameters.AddWithValue("@Description", animal.Description);
                command.Parameters.AddWithValue("@Category", animal.Category);
                command.Parameters.AddWithValue("@Area", animal.Area);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateAnimal(int Id, PutAdnimal putAdnimal)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UPDATE Animals SET Name = @Name, Description = @Description, Category = @Category, Area = @Area WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Name", putAdnimal.Name);
                command.Parameters.AddWithValue("@Description", putAdnimal.Description);
                command.Parameters.AddWithValue("@Category", putAdnimal.Category);
                command.Parameters.AddWithValue("@Area", putAdnimal.Area);
                
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

            }
        }
        
        public async Task DeleteAnimal(int Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand($"DELETE from Animals where Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", Id);
                
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
               
            }
        }
        
        public async Task<bool> AnimalExists(int Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT COUNT(1) FROM Animals WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", Id);
                await connection.OpenAsync();
                
                int count = (int)await command.ExecuteScalarAsync();
                return count > 0;
            }
        }

    }
}
