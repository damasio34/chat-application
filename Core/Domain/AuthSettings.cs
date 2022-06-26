namespace ChatApplication.Core.Domain
{
    public class AuthSettings
    {
        public string Secret { get; set; }
        public int ExpiresInHours { get; set; }
    }
}
