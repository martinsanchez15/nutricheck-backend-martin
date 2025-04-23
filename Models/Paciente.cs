using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NutriCheck.Models
{
    public class Paciente
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? Nombre { get; set; }
        public int Edad { get; set; }
        public string? Genero { get; set; }
        public float Altura { get; set; }
        public float Peso { get; set; }
        public string? Objetivo { get; set; }

        // Este también lo podés convertir en string si usás MongoDB para los nutricionistas
        public string? NutricionistaId { get; set; }

        public List<Preferencia> Preferencias { get; set; } = new();
    }
}
