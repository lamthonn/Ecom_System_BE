using Azure;
using Ecom.Context;
using Ecom.Dto;
using Ecom.Entity;
using Ecom.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Ecom.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(AppDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<string> Register(accountDto request)
        {
            try
            {
                var userDuplicate = _context.account.FirstOrDefault(x => x.tai_khoan == request.tai_khoan);
                if (userDuplicate != null)
                {
                    throw new Exception("Tài khoản đã tồn tại");
                }

                // Tạo Salt ngẫu nhiên cho mật khẩu
                byte[] salt = GenerateSalt();

                // Mã hóa mật khẩu sử dụng PBKDF2 và salt
                string hashPassword = GetPBKDF2(request.mat_khau, salt);

                var newAccount = new account
                {
                    id = Guid.NewGuid(),
                    tai_khoan = request.tai_khoan,
                    mat_khau = hashPassword,
                    salt = Convert.ToBase64String(salt), // Lưu Salt vào CSDL
                    ten = request.ten,
                    dia_chi = request.dia_chi,
                    ngay_sinh = request.ngay_sinh,
                    gioi_tinh = request.gioi_tinh ?? true,
                    email = request.email,
                    trang_thai = request.trang_thai ?? true,
                    so_dien_thoai = request.so_dien_thoai,
                    is_super_admin = request.is_super_admin ? request.is_super_admin : false,
                    Created = DateTime.Now,
                    CreatedBy = request.tai_khoan,
                };
                _context.Add(newAccount);
                _context.SaveChanges();

                return Task.FromResult("Đăng ký thành công!");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        public static string GetPBKDF2(string password, byte[] salt, int iterations = 10000)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                return Convert.ToBase64String(pbkdf2.GetBytes(32)); // 32 bytes for the hash output
            }
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[16]; // Salt 16 bytes
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public Task<string> Login(accountDto request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.tai_khoan) && !string.IsNullOrEmpty(request.mat_khau))
                {
                    if (request.is_super_admin == false)
                    {
                        var user = _context.account.FirstOrDefault(x => x.tai_khoan == request.tai_khoan && x.mat_khau == GetMD5(request.mat_khau));
                        if (user != null)
                        {
                            var claims = new List<Claim> {
                                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]!),
                                new Claim("id", user.id.ToString()),
                                new Claim("tai_khoan",user.tai_khoan),
                                new Claim("role", user.is_super_admin.ToString()! )
                            };
                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
                            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                            var token = new JwtSecurityToken(
                                _configuration["Jwt:Issuer"],
                                _configuration["Jwt:Audience"],
                                claims,
                                expires: DateTime.UtcNow.AddMinutes(120),
                                signingCredentials: signIn
                            );
                            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                            return Task.FromResult(jwtToken);
                        }
                        else
                        {
                            return Task.FromResult("Tài khoản và mật khẩu không đúng!");
                        }
                    }
                    else
                    {
                        return Task.FromResult("Tài khoản và mật khẩu không đúng!");
                    }
                }
                else
                {
                    return Task.FromResult("Tài khoản và mật khẩu không được để trống!");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        async public Task<loginDto> LoginAdmin(accountDto request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.tai_khoan) || !string.IsNullOrEmpty(request.mat_khau))
                {
                    if (request.is_super_admin == true)
                    {
                        var user = _context.account.FirstOrDefault(x => x.tai_khoan == request.tai_khoan);

                        if (user != null)
                        {
                            // Tạo lại hash mật khẩu từ mật khẩu người dùng nhập vào và salt trong DB
                            var salt = Convert.FromBase64String(user.salt!); // Salt đã lưu trong DB
                            var hashedPassword = GetPBKDF2(request.mat_khau, salt);
                            if (hashedPassword == user.mat_khau)
                            {
                                var jwtToken = GenerateJwtToken(user);
                                var refreshToken = GenerateRefreshToken();

                                // Lưu Refresh Token vào DB
                                user.RefreshToken = refreshToken;
                                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(3); 
                                _context.Update(user);
                                await _context.SaveChangesAsync();
                                return new loginDto { token = jwtToken, refreshToken = refreshToken };
                            }
                            else
                            {
                                throw new Exception("Mật khẩu không đúng");
                            }
                        }
                        else
                        {
                            throw new Exception("Không tìm thấy tài khoản");
                        }
                    }
                    else
                    {
                        throw new Exception("Tài khoản và mật khẩu không đúng");
                    }
                }
                else
                {
                    throw new Exception("Tài khoản và mật khẩu không được để trống");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private string GenerateJwtToken(account user)
        {
            var claims = new List<Claim> {
        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]!),
        new Claim("id", user.id.ToString()),
        new Claim("tai_khoan", user.tai_khoan),
        new Claim("role", user.is_super_admin.ToString())
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: signIn
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        async public Task<loginDto> RefreshToken(RefreshTokenRequest refreshToken)
        {
            var user = _context.account.FirstOrDefault(x => x.RefreshToken == refreshToken.RefreshToken);

            if (user == null )
            {
                return new loginDto { errrorMessage = "Refresh Token không hợp lệ" };
            }

            if ( user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                return new loginDto { errrorMessage = "Refresh Token đã hết hạn" };
            }

            var newAccessToken = GenerateJwtToken(user);
            //var newRefreshToken = GenerateRefreshToken();

            //user.RefreshToken = newRefreshToken;
            //user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(1);

            //_context.Update(user);
            //await _context.SaveChangesAsync();
            return new loginDto { token = newAccessToken, refreshToken = user.RefreshToken };
        }
    }
}
