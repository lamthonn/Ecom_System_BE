using AutoMapper;
using backend_v3.Models;
using Ecom.Context;
using Ecom.Dto.QuanLySanPham;
using Ecom.Dto.VanHanh;
using Ecom.Interfaces;

namespace Ecom.Services
{
    public class DonHangService : IDonHangService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public DonHangService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

       public async Task<PaginatedList<DonHangDto>> GetAll(DonHangDto request)
{
    try
    {
        // Thay vì ToList(), hãy để IQueryable
        var query = _context.don_hang.Where(x => x.trang_thai == request.trang_thai);

        // Ánh xạ dữ liệu trước khi đưa vào phân trang
        var dataDto = _mapper.ProjectTo<DonHangDto>(query);

        // Truyền IQueryable vào PaginatedList
        var result = await PaginatedList<DonHangDto>.Create(dataDto, request.pageNumber, request.pageSize);
        return result;
    }
    catch (Exception ex)
    {
        throw new Exception(ex.Message);
    }
}
    }
}
