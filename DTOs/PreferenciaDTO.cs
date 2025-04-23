namespace NutriCheck.DTOs
{
    public class PreferenciaDTO
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public int PacienteId { get; set; }
        public string NombrePaciente { get; set; } = string.Empty;
    }
}
