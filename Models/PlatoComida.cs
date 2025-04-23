namespace NutriCheck.Models
{
    public class PlatoComida
    {
        public int Id { get; set; }
        public int NutricionistaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Ingredientes { get; set; } = string.Empty;
        public string? Receta { get; set; }
        public int CaloriasAprox { get; set; }
        public int ProteinasAprox { get; set; }

        public Nutricionista? Nutricionista { get; set; }
    }
}
