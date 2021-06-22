namespace Karia.Api.Models
{
    public class AuthenticationResultDto
    {
        public string Token { get; set; }
        public string Type { get; set; }
    }

    public enum AuthType
    {
        Register,
        Login
    }
    
}