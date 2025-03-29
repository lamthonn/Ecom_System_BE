using AutoMapper;
using Ecom.Context;
using Ecom.Dto;
using Ecom.Entity;
using Ecom.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecom.Services
{
    public class NganHangService : INganHangService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public NganHangService(IMapper mapper, AppDbContext context, IHttpContextAccessor httpContextAccessor) 
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        private Guid GetCurrentAccountId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                throw new Exception("Không thể truy xuất HttpContext");

            var userIdClaim = httpContext.User.FindFirst("id");
            if (userIdClaim == null)
                throw new Exception("Không tìm thấy ID người dùng trong token");

            var accountId = Guid.Parse(userIdClaim.Value);

            return accountId;
        }
        public async Task<NganHangDto> AddBankAccountAsync(NganHangDto dto)
        {
            // Ánh xạ từ DTO sang entity
            var bankAccount = _mapper.Map<ngan_hang>(dto);

            // Gán các giá trị bổ sung
            bankAccount.id = Guid.NewGuid();
            bankAccount.account_id = GetCurrentAccountId(); // Gán account_id từ token

            // Nếu tài khoản này được đặt là mặc định, đặt các tài khoản khác thành không mặc định
            if (bankAccount.is_default)
            {
                var existingAccounts = await _context.ngan_hang
                    .Where(b => b.account_id == bankAccount.account_id)
                    .ToListAsync();
                existingAccounts.ForEach(b => b.is_default = false);
            }

            // Thêm vào cơ sở dữ liệu
            _context.ngan_hang.Add(bankAccount);
            await _context.SaveChangesAsync();

            // Ánh xạ lại sang DTO để trả về
            return _mapper.Map<NganHangDto>(bankAccount);
        }
        public async Task SetDefaultBankAccountAsync(Guid id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var accountId = GetCurrentAccountId();

                // Tìm tài khoản ngân hàng cần đặt làm mặc định
                var bankAccount = await _context.ngan_hang
                    .FirstOrDefaultAsync(b => b.id == id && b.account_id == accountId);
                if (bankAccount == null)
                {
                    throw new Exception("Không tìm thấy tài khoản ngân hàng hoặc bạn không có quyền truy cập.");
                }

                // Nếu tài khoản đã là mặc định, không cần làm gì
                if (bankAccount.is_default)
                {
                    return;
                }

                // Đặt tài khoản hiện tại đang mặc định thành không mặc định
                await _context.ngan_hang
                    .Where(b => b.account_id == accountId && b.is_default)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.is_default, false));

                // Đặt tài khoản được chọn thành mặc định
                bankAccount.is_default = true;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Lỗi khi đặt tài khoản ngân hàng mặc định: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }
    }
}
