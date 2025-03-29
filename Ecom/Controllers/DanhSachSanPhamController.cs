using backend_v3.Models;
using Ecom.Dto.QuanLySanPham;
using Ecom.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhSachSanPhamController : ControllerBase
    {
        private readonly ISanPhamService _service;
        public DanhSachSanPhamController(ISanPhamService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-all")]
        public Task<PaginatedList<SanPhamDto>> GetAll([FromQuery] SanPhamDto request)
        {
            try
            {
                return _service.GetAll(request);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        
        [HttpGet]
        [Route("get-all-sku")]
        public Task<PaginatedList<SanPhamDto>> GetAllSKU([FromQuery] SanPhamDto request)
        {
            try
            {
                return _service.GetAllSKU(request);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        
        [HttpGet]
        [Route("get-by-ma/{ma}")]
        public Task<List<SanPhamDto>> GetByMa([FromRoute] string ma)
        {
            try
            {
                return _service.GetByMa(ma);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        [HttpPost]
        [Route("add-cover-image")]
        public Task<string> AddCoverImage(IFormFile file)
        {
            try
            {
                return _service.SaveImageFileCoverPhoto(file);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        
        [HttpPost]
        [Route("add-list-image")]
        public Task<List<string>> AddListImage(List<IFormFile> files)
        {
            try
            {
                return _service.SaveMutiImageFile(files);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        [HttpPost]
        [Route("create")]
        public Task<List<SanPhamDto>> Create(List<SanPhamDto> request)
        {
            try
            {
                return _service.create(request);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        
        [HttpPost]
        [Route("add-image-files")]
        public async Task<ActionResult> AddImageFile(AddListImageProps req)
        {
            try
            {
                _service.AddListImage(req);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut]
        [Route("update-san-pham")]
        public async Task<ActionResult> Update(EditSanPhamRequest request)
        {
            try
            {
                await _service.Edit( request);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        [HttpDelete]
        [Route("delete/{ma}")]
        public async Task<ActionResult> Delete(Guid ma)
        {
            try
            {
                _service.Delete( ma);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete-any")]
        public async Task<ActionResult> DeleteAny(List<Guid> mas)
        {
            try
            {
                _service.DeleteAny(mas);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [Route("export")]
        public IActionResult DownloadExcel()
        {
            try
            {
                byte[] fileContents = _service.ExportToExcel();
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "danh_sach_san_pham.xlsx");
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
    }
}
