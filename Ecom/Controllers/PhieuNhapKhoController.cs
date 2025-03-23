using AutoMapper;
using Ecom.Dto.VanHanh;
using Ecom.Entity;
using Ecom.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Controllers
{
    [Route("api/phieu-nhap-kho")]
    [ApiController]
    public class PhieuNhapKhoController : GenericController<phieu_nhap_kho, PhieuNhapKhoDto>
    {
        private readonly IPhieuNhapKhoService _service;
        public PhieuNhapKhoController(IGenericRepository<phieu_nhap_kho> repository, IMapper mapper, IPhieuNhapKhoService service)
                    : base(repository, mapper) { 
            _service = service;
        }

        //xuất excel
        [HttpPost]
        [Route("export")]
        public IActionResult DownloadExcel()
        {
            try
            {
                byte[] fileContents = _service.ExportToExcel();
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "danh_sach_phieu_nhap_kho.xlsx");
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
    }
}
