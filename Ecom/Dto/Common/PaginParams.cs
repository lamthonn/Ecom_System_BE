namespace backend_v3.Dto.Common
{
    public class PaginParams
    {
        public string? keySearch {  get; set; }
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
    }
}
