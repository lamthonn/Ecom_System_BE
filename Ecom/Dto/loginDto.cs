namespace Ecom.Dto
{
    public class loginDto
    {
        public string? token { get; set; }
        public string? refreshToken { get; set; }
        public string? errrorMessage { get; set; }
    }

    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
    }
}
