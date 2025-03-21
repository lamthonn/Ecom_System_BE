﻿using AutoMapper;
using backend_v3.Models;
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

            CreateMap<san_pham, SanPhamDto>()
                .ForMember(dest => dest.ds_anh_san_pham, opt => opt.MapFrom(src => src.ds_anh_san_pham!.Where(x=> x.ma_san_pham == src.ma_san_pham))); 
            CreateMap<SanPhamDto, san_pham>();


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
