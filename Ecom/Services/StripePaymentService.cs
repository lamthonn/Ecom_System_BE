using Ecom.Dto;
using Ecom.Entity;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.Text;

namespace Ecom.Services
{
    public class StripePaymentService
    {
        public StripePaymentService(IOptions<StripeSettings> stripeSettings)
        {
            StripeConfiguration.ApiKey = stripeSettings.Value.SecretKey;
        }

        public Session CreateCheckoutSession(PaymentParam param)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>{"card"},
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long?)(param.priceInCents), //Convert to long and multiply by 100
                        Currency = "vnd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = GenerateOrderId(),
                        },
                    },
                    Quantity = 1,
                },
            },
                Mode = "payment",
                SuccessUrl = param.successUrl,
                CancelUrl = param.cancelUrl,
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return session;
        }

        private static string GenerateOrderId()
        {
            // Lấy ngày hiện tại theo định dạng yyMMdd (VD: 220826)
            string datePart = DateTime.Now.ToString("yyMMdd");

            // Tạo một số ngẫu nhiên (6 chữ số)
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999);

            // Tạo một chuỗi ký tự ngẫu nhiên (VD: M5BM14B)
            string randomString = GenerateRandomString(7);

            // Ghép lại thành ID
            return $"{datePart}{randomNumber}{randomString}";
        }

        static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder result = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }
            return result.ToString();
        }
    }
}
