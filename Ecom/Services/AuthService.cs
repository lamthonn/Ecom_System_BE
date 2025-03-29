using AutoMapper;
using Azure;
using Ecom.Context;
using Ecom.Dto;
using Ecom.Entity;
using Ecom.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;

        public AuthService(AppDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
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

        async public Task<accountDetailDto> getDetailAcc()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                    throw new Exception("Không thể truy xuất HttpContext");

                var userIdClaim = httpContext.User.FindFirst("id");
                if (userIdClaim == null)
                    throw new Exception("Không tìm thấy ID người dùng trong token");

                var userId = Guid.Parse(userIdClaim.Value);
                var user = await _context.account.FirstOrDefaultAsync(x => x.id == userId);

                if (user == null)
                    throw new Exception("Không tìm thấy tài khoản");
                var listNganHang = _context.ngan_hang.Where(x => x.account_id == userId);
                
                var result = _mapper.Map<accountDetailDto>(user);
                result.listNganHangs = _mapper.Map<List<NganHangDto>>(listNganHang);
                return result;
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
        public Dictionary<string, string> DecodeJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                throw new ArgumentException("Invalid token");

            var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
            return claims;
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

        async public Task<string> UpdatePhone(UpdatePhoneDto request)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                    throw new Exception("Không thể truy xuất HttpContext");

                var userIdClaim = httpContext.User.FindFirst("id");
                if (userIdClaim == null)
                    throw new Exception("Không tìm thấy ID người dùng trong token");

                var userId = Guid.Parse(userIdClaim.Value);
                var user = await _context.account.FirstOrDefaultAsync(x => x.id == userId);

                if (user == null)
                    throw new Exception("Không tìm thấy tài khoản");

                // Cập nhật số điện thoại
                user.so_dien_thoai = request.so_dien_thoai;
                user.LastModified = DateTime.Now;
                user.LastModifiedBy = user.tai_khoan;

                _context.Update(user);
                await _context.SaveChangesAsync();

                return "Cập nhật số điện thoại thành công!";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Cập nhật email
        async public Task<string> UpdateEmail(UpdateEmailDto request)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                    throw new Exception("Không thể truy xuất HttpContext");

                var userIdClaim = httpContext.User.FindFirst("id");
                if (userIdClaim == null)
                    throw new Exception("Không tìm thấy ID người dùng trong token");

                var userId = Guid.Parse(userIdClaim.Value);
                var user = await _context.account.FirstOrDefaultAsync(x => x.id == userId);

                if (user == null)
                    throw new Exception("Không tìm thấy tài khoản");

                // Kiểm tra email có bị trùng không
                var emailDuplicate = await _context.account.FirstOrDefaultAsync(x => x.email == request.email && x.id != userId);
                if (emailDuplicate != null)
                    throw new Exception("Email đã được sử dụng bởi tài khoản khác");

                // Cập nhật email
                user.email = request.email;
                user.LastModified = DateTime.Now;
                user.LastModifiedBy = user.tai_khoan;

                _context.Update(user);
                await _context.SaveChangesAsync();

                return "Cập nhật email thành công!";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Cập nhật mật khẩu
        async public Task<string> UpdatePassword(UpdatePasswordDto request)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                    throw new Exception("Không thể truy xuất HttpContext");

                var userIdClaim = httpContext.User.FindFirst("id");
                if (userIdClaim == null)
                    throw new Exception("Không tìm thấy ID người dùng trong token");

                var userId = Guid.Parse(userIdClaim.Value);
                var user = await _context.account.FirstOrDefaultAsync(x => x.id == userId);

                if (user == null)
                    throw new Exception("Không tìm thấy tài khoản");

                // Kiểm tra mật khẩu cũ
                var salt = Convert.FromBase64String(user.salt!);
                var hashedOldPassword = GetPBKDF2(request.oldPassword, salt);
                if (hashedOldPassword != user.mat_khau)
                    throw new Exception("Mật khẩu cũ không đúng");

                // Tạo salt mới cho mật khẩu mới
                byte[] newSalt = GenerateSalt();
                string newHashedPassword = GetPBKDF2(request.newPassword, newSalt);

                // Cập nhật mật khẩu và salt mới
                user.mat_khau = newHashedPassword;
                user.salt = Convert.ToBase64String(newSalt);
                user.LastModified = DateTime.Now;
                user.LastModifiedBy = user.tai_khoan;

                _context.Update(user);
                await _context.SaveChangesAsync();

                return "Cập nhật mật khẩu thành công!";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
