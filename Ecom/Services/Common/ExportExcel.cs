using Ecom.Context;

namespace Ecom.Services.Common
{
    public class ExportExcel
    {
        private readonly AppDbContext _context;
        public ExportExcel(AppDbContext context)
        {
            _context = context;
        }


    }
}
