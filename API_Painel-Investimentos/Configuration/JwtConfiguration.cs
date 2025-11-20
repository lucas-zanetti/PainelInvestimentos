namespace API_Painel_Investimentos.Configuration
{
    public class JwtConfiguration
    {
        public required string Key { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public short ExpirationTimeMinutes { get; set; }
    }
}
