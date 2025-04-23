namespace NutriCheck.Models
{
    public class Comida
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public string? Tipo { get; set; } // Desayuno, Almuerzo, Merienda, Cena
        public string? Nombre { get; set; } // Ej: "Tostadas con queso"
        public int Calorias { get; set; }
        public DateTime Fecha { get; set; }

        // Navegación opcional
        public Paciente? Paciente { get; set; }
    }
}
