using AutoMapper;
using backend_v3.Dto.Common;
using Ecom.Entity;

namespace Ecom.Dto.QuanLySanPham
{
    public class SanPhamDto : PaginParams
    {
        public Guid? id { get; set; }
        public Guid? danh_muc_id { get; set; }
        public string? ma_san_pham { get; set; }
        public string? ten_san_pham { get; set; }
        public string? mo_ta { get; set; }
        public string? xuat_xu { get; set; }
        public int? luot_ban { get; set; }
        public int? so_luong { get; set; }
        public string? sku { get; set; }
        public string? duong_dan_anh_bia { get; set; }
        public string? mau_sac { get; set; }
        public string? size { get; set; }
        public decimal? gia { get; set; }
        public decimal? khuyen_mai { get; set; }
        public List<string>? ds_anh_san_pham { get; set; }
        public List<PhanLoai>? ls_phan_loai { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public bool? is_active { get; set; } = true;
    }

    public class AddListImageProps
    {
        public List<string>? filePath { get; set; }
        public string? ma { get; set; }
    }
    
    public class PhanLoai
    {
        public string? ten_phan_loai { get; set; }
        public List<string>? phan_loai { get; set; }
    }

    public class EditSanPhamRequest
    {
        public string? ma { get; set; }
        public IFormFile? anh_bia { get; set; }
        public List<IFormFile>? ls_anh_san_pham{ get; set; }
        public string? data_san_pham{ get; set; }
    }

}
