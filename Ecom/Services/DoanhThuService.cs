using Ecom.Context;
using Ecom.Dto;
using Ecom.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Services
{
    public class DoanhThuService : IDoanhThuService
    {
        private readonly AppDbContext _context;
        public DoanhThuService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<RevenueStatsDto> GetRevenueStats(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            try
            {
                // Mặc định: lấy dữ liệu 30 ngày gần nhất nếu không có tham số
                startDate ??= DateTime.UtcNow.AddDays(-30);
                endDate ??= DateTime.UtcNow;

                // Lọc đơn hàng theo khoảng thời gian
                var ordersQuery = _context.don_hang
                    .Where(o => o.ngay_mua >= startDate && o.ngay_mua <= endDate);

                // Tổng doanh thu
                var totalRevenue = await ordersQuery.SumAsync(o => o.thanh_tien);

                // Doanh thu theo ngày
                var revenueByDate = await ordersQuery
                    .GroupBy(o => o.ngay_mua.Date)
                    .Select(g => new RevenueByDateDto
                    {
                        Date = g.Key.ToString("dd/MM/yyyy"),
                        Revenue = g.Sum(o => o.thanh_tien)
                    })
                    .OrderBy(r => r.Date)
                    .ToListAsync();

                // Doanh thu theo tháng
                var revenueByMonth = await ordersQuery
                    .GroupBy(o => new { o.ngay_mua.Year, o.ngay_mua.Month })
                    .Select(g => new RevenueByMonthDto
                    {
                        Month = $"{g.Key.Month:D2}/{g.Key.Year}",
                        Revenue = g.Sum(o => o.thanh_tien)
                    })
                    .OrderBy(r => r.Month)
                    .ToListAsync();

                // Doanh thu theo đơn hàng
                var revenueByOrder = await ordersQuery
                    .Select(o => new RevenueByOrderDto
                    {
                        OrderId = o.ma_don_hang,
                        Revenue = o.thanh_tien
                    })
                    .OrderByDescending(r => r.Revenue)
                    .ToListAsync();

                // Doanh thu theo sản phẩm (giả sử có chi tiết đơn hàng)
                var revenueByProduct = await _context.chi_tiet_don_hang
                    .Where(ct => ordersQuery.Any(o => o.id == ct.don_hang_id))
                    .GroupBy(ct => new { ct.san_pham_id, ct.San_pham.ten_san_pham })
                    .Select(g => new RevenueByProductDto
                    {
                        ProductId = g.Key.san_pham_id.ToString(),
                        ProductName = g.Key.ten_san_pham,
                        Revenue = g.Sum(ct => ct.don_gia * ct.so_luong)
                    })
                    .OrderByDescending(r => r.Revenue)
                    .ToListAsync();

                // Kết quả trả về
                var result = new RevenueStatsDto
                {
                    TotalRevenue = totalRevenue,
                    RevenueByDate = revenueByDate,
                    RevenueByMonth = revenueByMonth,
                    RevenueByOrder = revenueByOrder,
                    RevenueByProduct = revenueByProduct
                };

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
