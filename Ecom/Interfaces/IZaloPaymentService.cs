using Ecom.Dto.VanHanh;
using Ecom.Entity;
using Ecom.Services;
using Newtonsoft.Json.Linq;

namespace Ecom.Interfaces
{
    public interface IZaloPaymentService
    {
        Task<JObject> CreateZaloPayOrderAsync(DonHangDto order);
        Task<string> HandleZaloPayCallbackAsync(string callbackData);
        public Task<bool> CreateOrder(DonHangDto order, Guid? lichSuGiaoDichId, Guid? accountId, int? trang_thai);
    }
}
