using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NutriCheck.Models;
using NutriCheck.Settings;

namespace NutriCheck.Data
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _db;
        public MongoDbService(IOptions<MongoDBSettings> opts)
        {
            var client = new MongoClient(opts.Value.ConnectionString);
            _db = client.GetDatabase(opts.Value.DatabaseName);
        }

        public IMongoCollection<Nutricionista> Nutricionistas =>
            _db.GetCollection<Nutricionista>("Nutricionistas");
        public IMongoCollection<Paciente> Pacientes =>
            _db.GetCollection<Paciente>("Pacientes");
        public IMongoCollection<Preferencia> Preferencias =>
            _db.GetCollection<Preferencia>("Preferencias");
        public IMongoCollection<Comida> Comidas =>
            _db.GetCollection<Comida>("Comidas");
        public IMongoCollection<Dieta> Dietas =>
            _db.GetCollection<Dieta>("Dietas");
    }
}
