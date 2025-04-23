namespace NutriCheck.Models
{
    public class Dieta
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int PlatoComidaId { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; } = string.Empty; // Desayuno, Almuerzo, Merienda, Cena

        public Paciente? Paciente { get; set; }
        public PlatoComida? Plato { get; set; }
    }
}
