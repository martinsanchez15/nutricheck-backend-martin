using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NutriCheck.Models;

namespace NutriCheck.Data
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        // Ejemplo para acceder a una colección
        public IMongoCollection<Paciente> Pacientes =>
            _database.GetCollection<Paciente>("Pacientes");

        // Podés agregar otras colecciones como:
        // public IMongoCollection<Comida> Comidas => ...
    }
}
