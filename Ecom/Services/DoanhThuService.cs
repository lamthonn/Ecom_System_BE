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
                // Mặc định lấy dữ liệu 30 ngày gần nhất nếu không có tham số
                startDate ??= DateTime.UtcNow.AddDays(-30);
                endDate ??= DateTime.UtcNow;

                // Lọc đơn hàng theo khoảng thời gian
                var ordersQuery = _context.don_hang
                    .Where(o => o.ngay_mua >= startDate && o.ngay_mua <= endDate);

                // Tổng doanh thu
                var totalRevenue = await ordersQuery.SumAsync(o => o.thanh_tien);

                // Doanh thu theo ngày
                // Tạo danh sách tất cả các ngày trong khoảng startDate - endDate
                var allDates = Enumerable.Range(0, (endDate - startDate).Value.Days + 1)
                    .Select(i => startDate.Value.AddDays(i).Date)
                    .ToList();

                // Lấy doanh thu theo ngày từ database
                var revenueByDateRaw = await ordersQuery
                    .Where(o => o.ngay_mua >= startDate && o.ngay_mua <= endDate)
                    .GroupBy(o => o.ngay_mua.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        Revenue = g.Sum(o => o.thanh_tien)
                    })
                    .ToListAsync();

                // Ghép danh sách tất cả các ngày với dữ liệu doanh thu từ DB
                var revenueByDate = allDates
                    .Select(date => new RevenueByDateDto
                    {
                        Date = date.ToString("dd/MM/yyyy"),
                        Revenue = revenueByDateRaw.FirstOrDefault(r => r.Date == date)?.Revenue ?? 0
                    })
                    .ToList();


                // Doanh thu theo tháng (tách phần format ra khỏi truy vấn LINQ)
                var revenueByMonthRaw = await ordersQuery
                    .GroupBy(o => new { o.ngay_mua.Year, o.ngay_mua.Month })
                    .Select(g => new
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        Revenue = g.Sum(o => o.thanh_tien)
                    })
                    .OrderBy(r => r.Year).ThenBy(r => r.Month)
                    .ToListAsync();

                var revenueByMonth = revenueByMonthRaw
                    .Select(r => new RevenueByMonthDto
                    {
                        Month = $"{r.Month:D2}/{r.Year}", // Format trên C#
                        Revenue = r.Revenue
                    })
                    .ToList();

                // Doanh thu theo đơn hàng
                var revenueByOrder = await ordersQuery
                    .Select(o => new RevenueByOrderDto
                    {
                        OrderId = o.ma_don_hang,
                        Revenue = o.thanh_tien
                    })
                    .OrderByDescending(r => r.Revenue)
                    .ToListAsync();

                // Doanh thu theo sản phẩm
                var revenueByProduct = await _context.chi_tiet_don_hang
                 .Where(ct => ordersQuery.Any(o => o.id == ct.don_hang_id))
                 .Join(_context.san_pham,
                       ct => ct.san_pham_id,
                       sp => sp.id,
                       (ct, sp) => new { ct, sp })
                 .GroupBy(x => new { x.ct.san_pham_id, x.sp.ten_san_pham })
                 .Select(g => new
                 {
                     ProductId = g.Key.san_pham_id,
                     ProductName = g.Key.ten_san_pham,
                     Revenue = g.Sum(x => x.ct.thanh_tien)
                 })
                 .OrderByDescending(r => r.Revenue)
                 .ToListAsync();


                var revenueByProductDto = revenueByProduct
                    .Select(r => new RevenueByProductDto
                    {
                        ProductId = r.ProductId.ToString(),
                        ProductName = r.ProductName,
                        Revenue = r.Revenue
                    })
                    .ToList();

                // Kết quả trả về
                var result = new RevenueStatsDto
                {
                    TotalRevenue = totalRevenue,
                    RevenueByDate = revenueByDate,
                    RevenueByMonth = revenueByMonth,
                    RevenueByOrder = revenueByOrder,
                    RevenueByProduct = revenueByProductDto
                };

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy dữ liệu doanh thu", ex);
            }
        }

        public async Task<DashboardStatsDto> GetDashboardStats(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                startDate ??= DateTime.UtcNow.AddDays(-30);
                endDate ??= DateTime.UtcNow;

                var ordersQuery = _context.don_hang
                    .Where(o => o.ngay_mua >= startDate && o.ngay_mua <= endDate);

                var totalRevenue = await ordersQuery.SumAsync(o => o.thanh_tien);
                var totalOrders = await ordersQuery.CountAsync();
                var totalCustomers = await _context.account
                    .Where(x => x.Created >= startDate && x.Created <=endDate && x.is_super_admin == false)
                    .CountAsync();
                var totalProducts = await _context.san_pham
                    .Where(x => x.Created >= startDate && x.Created <= endDate)
                    .CountAsync();

                var refundOrders = await ordersQuery.Where(o => o.trang_thai == 5).CountAsync();
                var refundRate = totalOrders > 0 ? (double)refundOrders / totalOrders * 100 : 0;

                return new DashboardStatsDto
                {
                    TotalRevenue = totalRevenue,
                    TotalOrders = totalOrders,
                    TotalCustomers = totalCustomers,
                    TotalProducts = totalProducts,
                    RefundRate = refundRate
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy dữ liệu thống kê Dashboard", ex);
            }
        }
    }
}
