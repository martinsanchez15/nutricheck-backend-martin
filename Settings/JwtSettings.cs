namespace NutriCheck.Settings
{
    public class JwtSettings
    {
        public string Secret       { get; set; } = null!;
        public int    ExpiryHours  { get; set; }
    }
}
