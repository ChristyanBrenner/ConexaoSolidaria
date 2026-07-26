namespace Domain.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public DateTime Expiracao { get; set; }
    }
}