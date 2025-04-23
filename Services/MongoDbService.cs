using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NutriCheck.Models;
using NutriCheck.Settings;

namespace NutriCheck.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Paciente> Pacientes =>
            _database.GetCollection<Paciente>("Pacientes");

        // Podés agregar más colecciones así:
        // public IMongoCollection<OtroModelo> Otros => _database.GetCollection<OtroModelo>("NombreColeccion");
    }
}
