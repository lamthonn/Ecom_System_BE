using AutoMapper;
using backend_v3.Models;
using Ecom.Dto.KhachHang;
using Ecom.Dto.ProductTest;
using Ecom.Dto.QuanLySanPham;
using Ecom.Entity;
using global::AutoMapper;
using System.Collections.Generic;

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
