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
        
        //tạo phiếu nhập kho
        [HttpPost]
        [Route("create")]
        public Task<PhieuNhapKhoDto> Created(PhieuNhapKhoDto request)
        {
            try
            {
                return _service.Create(request);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        
        [HttpGet]
        [Route("get-by-id/{id}")]
        public Task<PhieuNhapKhoDto> GetById(string id)
        {
            try
            {
                return _service.GetById(id);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        
        [HttpPost]
        [Route("edit/{id}")]
        public Task<PhieuNhapKhoDto> Edit(string id, PhieuNhapKhoDto request)
        {
            try
            {
                return _service.Edit(id, request);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }

        [HttpPut]
        [Route("xu-ly/{id}")]
        public IActionResult XuLy(string id)
        {
            try
            {
                _service.XuLyPhieuNhap(id);
                return Ok();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
    }
}
