using Ecom.Context;

namespace Ecom.Services.Common
{
    public class SaveFileCommon
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SaveFileCommon(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<string> SaveImageFileCommon(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception("File không hợp lệ");
            }
            // Đảm bảo đường dẫn root không bị null
            string rootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string uploadsFolder = Path.Combine(rootPath, folderName);
            // Tạo thư mục nếu chưa có
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Tạo đường dẫn lưu file
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Lưu file vào wwwroot/uploads
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Trả về đường dẫn ảnh
            string fileUrl = $"{folderName}/{uniqueFileName}";
            return fileUrl;
        }

        public async Task<List<string>> SaveMultipleImageFilesCommon(List<IFormFile> files, string folderName)
        {
            List<string> fileUrls = new List<string>();

            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                {
                    throw new Exception("File không hợp lệ");
                }

                // Đảm bảo đường dẫn root không bị null
                string rootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                string uploadsFolder = Path.Combine(rootPath, folderName);

                // Tạo thư mục nếu chưa có
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // Tạo đường dẫn lưu file
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Lưu file vào wwwroot/uploads
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Trả về đường dẫn ảnh
                string fileUrl = $"{folderName}/{uniqueFileName}";
                fileUrls.Add(fileUrl);
            }

            return fileUrls;
        }

        public bool checkDuplicateFile(IFormFile file)
        {
            // Kiểm tra trùng file dựa trên tên file
            //var existingFile = _context.Files.FirstOrDefault(f => f.FileName == file.FileName);
            //return existingFile != null;
            return true;
        }
    }
}
