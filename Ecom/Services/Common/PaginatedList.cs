using System.Security.Principal;

namespace backend_v3.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; }
        public int? PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecord { get; set; }
        public int PageSize { get; set; }

        public PaginatedList(List<T> items, int count, int? pageIndex, int pageSize)
        {
            TotalRecord = count;
            PageSize = pageSize;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;
        }

        public static PaginatedList<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
