using AutoMapper;
using backend_v3.Models;
using Ecom.Dto;
using Ecom.Dto.KhachHang;
using Ecom.Dto.ProductTest;
using Ecom.Dto.QuanLySanPham;
using Ecom.Dto.VanHanh;
using Ecom.Entity;

namespace Ecom.AutoMapper
{    
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Định nghĩa ánh xạ các Entity sang DTO
            CreateMap<san_pham, ProductTestDto>()
                .ForMember(dest => dest.ma_san_pham_dto, opt => opt.MapFrom(src => src.ma_san_pham))
                .ReverseMap();

            // khách hàng
            CreateMap<account, KhachHangDto>()
                .ReverseMap();

            //sản phẩm
            CreateMap<san_pham, SanPhamDto>()
                .ForMember(dest => dest.ds_anh_san_pham, opt => opt.MapFrom(src => src.ds_anh_san_pham!.Where(x=> x.ma_san_pham == src.ma_san_pham))) 
                .ForMember(dest => dest.ten_danh_muc, opt => opt.MapFrom(src => src.danh_Muc!.ten_danh_muc)); 
            CreateMap<SanPhamDto, san_pham>();

            //đơn hàng
            CreateMap<don_hang, DonHangDto>();
            CreateMap<DonHangDto, don_hang>();
            CreateMap<chi_tiet_don_hang, ChiTietDonHangDto>()
                .ForMember(x => x.ten_san_pham, me => me.MapFrom(src => src.San_pham!.ten_san_pham));
            CreateMap<ChiTietDonHangDto, chi_tiet_don_hang>();

            // account detail
            CreateMap<accountDetailDto, account>().ReverseMap();

            // mã giảm giá
            CreateMap<MaGiamGiaDto, ma_giam_gia>()
                .ForMember(dest => dest.id, opt => opt.Ignore())
                .ForMember(dest => dest.bat_dau, opt => opt.MapFrom(src => src.thoi_gian[0]))
                .ForMember(dest => dest.ket_thuc, opt => opt.MapFrom(src => src.thoi_gian[1]));
            CreateMap<ma_giam_gia, MaGiamGiaDto>()
                .ForMember(dest => dest.thoi_gian, opt => opt.MapFrom(src => new List<DateTime?> { src.bat_dau, src.ket_thuc }));

            // chương trình mar
            CreateMap<ChuongTrinhMarDto, chuong_trinh_marketing>().ReverseMap();

            // ngân hàng
            CreateMap<NganHangDto, ngan_hang>().ReverseMap();
            //Phiếu nhập kho
            CreateMap<phieu_nhap_kho, PhieuNhapKhoDto>();
            CreateMap<PhieuNhapKhoDto, phieu_nhap_kho>();
            //chi tiết phiếu nhập
            CreateMap<chi_tiet_phieu_nhap_kho, ChiTietPhieuNhapDto>();
            CreateMap<ChiTietPhieuNhapDto, chi_tiet_phieu_nhap_kho>();

            // Định nghĩa ánh xạ chung cho PaginatedList<T>
            CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>))
                .ConvertUsing(typeof(PaginatedListConverter<,>));
        }
    }

    public class PaginatedListConverter<TSource, TDestination>
        : ITypeConverter<PaginatedList<TSource>, PaginatedList<TDestination>>
    {
        private readonly IMapper _mapper;

        public PaginatedListConverter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public PaginatedList<TDestination> Convert(
            PaginatedList<TSource> source,
            PaginatedList<TDestination> destination,
            ResolutionContext context)
        {
            var items = _mapper.Map<List<TDestination>>(source.Items);
            return new PaginatedList<TDestination>(items, source.TotalRecord, source.PageIndex, source.PageSize);
        }
    }

}
