using Ecom.Dto.VanHanh;

namespace Ecom.Dto
{
    public class PaymentParam
    {
        public string? stripeSessionId { get; set; }
        public string? successUrl { get; set; }
        public string? cancelUrl{ get; set; }
        public decimal? priceInCents { get; set; }
        public string? userId { get; set; }
        public DonHangDto? donHang { get; set; }
    }
}
