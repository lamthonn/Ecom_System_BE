namespace Ecom.Dto
{
    public class RevenueStatsDto
    {
        public decimal TotalRevenue { get; set; }
        public List<RevenueByDateDto> RevenueByDate { get; set; }
        public List<RevenueByMonthDto> RevenueByMonth { get; set; }
        public List<RevenueByOrderDto> RevenueByOrder { get; set; }
        public List<RevenueByProductDto> RevenueByProduct { get; set; }
    }

    public class RevenueByDateDto
    {
        public string Date { get; set; }
        public decimal Revenue { get; set; }
    }

    public class RevenueByMonthDto
    {
        public string Month { get; set; } // Định dạng: "MM/YYYY"
        public decimal Revenue { get; set; }
    }

    public class RevenueByOrderDto
    {
        public string OrderId { get; set; }
        public decimal Revenue { get; set; }
    }

    public class RevenueByProductDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? Revenue { get; set; }
    }
}