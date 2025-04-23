namespace NutriCheck.Models
{
    public class Preferencia
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string? PacienteId { get; set; } // sigue siendo string porque se recibe como string desde el frontend
        public Paciente? Paciente { get; set; }
    }
}
