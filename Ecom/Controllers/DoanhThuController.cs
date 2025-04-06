using Microsoft.AspNetCore.Mvc;
using Ecom.Entity;
using Ecom.Dto;
using Microsoft.EntityFrameworkCore;
using Ecom.Context;
using Ecom.Interfaces;
using backend_v3.Models;

namespace Ecom.Controllers
{
    [Route("api/doanh-thu")]
    [ApiController]
    public class DoanhThuController : ControllerBase
    {
        private readonly IDoanhThuService _service;

        public DoanhThuController(IDoanhThuService service)
        {
            _service = service;
        }

        [HttpGet("thong-ke")]
        public async Task<RevenueStatsDto> GetRevenueStats(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            try
            {              
                var result = await _service.GetRevenueStats(startDate, endDate);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("dashboard")]
        public async Task<DashboardStatsDto> GetDashboardStats(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            try
            {
                var result = await _service.GetDashboardStats(startDate, endDate);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("so-du")]
        public async Task<PaginatedList<LichSuGiaoDichDto>> GetDashboardStats(LichSuGiaoDichDto request)
        {
            try
            {
                var result = await _service.GetSoDu(request);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}