namespace MBD.Identity.Domain.DTO
{
    public class JwtConfiguration
    {
        public string Secret { get; set; }
        
        public int ExpiresInSeconds { get; set; }
        
        public int RefreshExpiresInSeconds { get; set; }
        
        public string Issuer { get; set; }
        
        public string Audience { get; set; }
    }
}