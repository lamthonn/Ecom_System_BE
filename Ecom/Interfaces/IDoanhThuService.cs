using Ecom.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Interfaces
{
    public interface IDoanhThuService
    {
        public Task<RevenueStatsDto> GetRevenueStats([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate);
        public Task<DashboardStatsDto> GetDashboardStats(DateTime? startDate, DateTime? endDate);
    }
}
