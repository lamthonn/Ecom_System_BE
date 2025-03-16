using Azure.Core;
using Ecom.Dto;
using Ecom.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace Ecom.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenController(IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public Task<string> Register(accountDto request)
        {
            try
            {
                var addedUser = _authService.Register(request);
                return addedUser;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPost]
        public Task<string> Login(accountDto request)
        {
            try
            {
                var addedUser = _authService.Login(request);
                return addedUser;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPost]
        public Task<loginDto> LoginAdmin(accountDto request)
        {
            try
            {
                var addedUser = _authService.LoginAdmin(request);
                return addedUser;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public string TestConnect()
        {
            try
            {
                return @$"Current Time: {DateTime.UtcNow}";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPost("refresh-token")]
        public Task<loginDto> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var addedUser = _authService.RefreshToken(request);
                return addedUser;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //[HttpGet("check-auth")]
        //public IActionResult CheckAuth()
        //{
        //    var token = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];
        //    if (string.IsNullOrEmpty(token))
        //    {
        //        return Unauthorized(new { message = "Chưa đăng nhập" });
        //    }

        //    return Ok(new { message = "Đã đăng nhập" });
        //}
    }
}
