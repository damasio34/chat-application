namespace ChatApplication.Core.Domain.DTOs
{
    public class LoginResultDTO
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public bool IsAuthenticated { get; set; } = false;
    }
}
