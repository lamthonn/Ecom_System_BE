using Ecom.Dto.QuanLySanPham;
using Ecom.Entity;
using AutoMapper;
using backend_v3.Models;

namespace Ecom.Dto.ProductTest
{
    public class ProductTestDto
    {
        public Guid id { get; set; }
        public string ma_san_pham_dto { get; set; }
        public string ten_san_pham { get; set; }
        public Guid danh_muc_id { get; set; }
        public string sku { get; set; }
    }

}
