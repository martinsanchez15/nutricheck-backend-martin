namespace NutriCheck.Models
{
    public class Nutricionista
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; } // Simple por ahora
        public string Rol { get; set; } = "Nutricionista"; // valor por defecto

    }

}
