using Ecom.Dto;

namespace Ecom.Interfaces
{
    public interface IAuthService
    {
        public Task<string> Register(accountDto request);
        public Task<string> Login(accountDto request);
        public Task<loginDto> LoginAdmin(accountDto request);
        public Task<loginDto> RefreshToken(RefreshTokenRequest request);

    }
}
