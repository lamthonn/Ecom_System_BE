using Ecom.Dto;

namespace Ecom.Interfaces
{
    public interface IAuthService
    {
        public Task<string> Register(accountDto request);
        public Task<loginDto> Login(accountDto request);
        public Task<loginDto> LoginAdmin(accountDto request);
        public Task<loginDto> RefreshToken(RefreshTokenRequest request);
        public Task<accountDetailDto> getDetailAcc();
        public Task<string> UpdatePhone(UpdatePhoneDto request);
        public Task<string> UpdateEmail(UpdateEmailDto request);
        public Task<string> UpdatePassword(UpdatePasswordDto request);
        public Task<bool> blockUser(Guid id);
        public Task<bool> UpdateUser(accountDetailDto model);
    }
}
