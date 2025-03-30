using backend_v3.Models;
using Ecom.Dto.QuanLySanPham;
using Ecom.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class DanhMucSanPhamController : ControllerBase
    {
        private readonly IDanhMucSanPhamService _service;
        public DanhMucSanPhamController(IDanhMucSanPhamService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-all")]
        public Task<PaginatedList<DanhMucDto>> GetAll([FromQuery]DanhMucDto request)
        {
            try
            {
                return  _service.GetAll(request);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        [HttpGet]
        [Route("get-by-id/{id}")]
        public Task<DanhMucDto> GetById([FromRoute] string id)
        {
            try
            {
                return _service.GetById(id);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        [Authorize]
        [HttpPost]
        [Route("create")]
        public Task<DanhMucDto> Create(DanhMucDto request)
        {
            try
            {
                return _service.create(request);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        [Authorize]
        [HttpPut]
        [Route("edit/{id}")]
        public void Edit(string id, DanhMucDto request)
        {
            try
            {
                _service.Edit(id, request);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        [Authorize]
        [HttpDelete]
        [Route("delete/{id}")]
        public void Delete(string id)
        {
            try
            {
                _service.Delete(id);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        [Authorize]
        [HttpDelete]
        [Route("delete-any")]
        public void DeleteAny([FromBody]List<string> ids)
        {
            try
            {
                _service.DeleteAny(ids);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        [Authorize]
        [HttpPost]
        [Route("export")]

        public IActionResult DownloadExcel()
        {
            try
            {
                byte[] fileContents = _service.ExportToExcel();
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhMucSanPham.xlsx");
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            
        }
    }
}
