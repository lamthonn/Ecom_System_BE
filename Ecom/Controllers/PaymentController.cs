using Ecom.Context;
using Ecom.Dto;
using Ecom.Dto.VanHanh;
using Ecom.Entity;
using Ecom.Interfaces;
using Ecom.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Ecom.Controllers.common
{
    [Route("api/thanh-toan")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly StripePaymentService _paymentService;
        private readonly AppDbContext _context;
        private readonly IZaloPaymentService _zaloPaymentService;

        public PaymentController(StripePaymentService paymentService, AppDbContext context, IZaloPaymentService zaloPaymentService)
        {
            _paymentService = paymentService;
            _context = context;
            _zaloPaymentService = zaloPaymentService;
        }


        [HttpPost("zalo-create-order")]
        public async Task<IActionResult> CreateZaloPayOrder([FromBody] DonHangDto order)
        {
            if (order == null)
            {
                return BadRequest("Invalid order data.");
            }

            try
            {
                var response = await _zaloPaymentService.CreateZaloPayOrderAsync(order);

                if ((int?)response["return_code"] == 1 && response["order_url"] != null)
                {
                    string orderUrl = response["order_url"]!.ToString();

                    return Ok(new
                    {
                        message = "Tạo đơn hàng thành công",
                        paymentUrl = orderUrl
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        message = "Tạo đơn hàng thất bại",
                        error_code = response["return_code"]?.ToString(),
                        error_message = response["return_message"]?.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Lỗi khi tạo đơn hàng ZaloPay: {ex.Message}" });
            }
        }
        [HttpPost("create-order-tien-mat")]
        public async Task<bool> CreateOrder([FromBody] DonHangDto order)
        {
            try
            {
                var response = await _zaloPaymentService.CreateOrder(order, null, null, null);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo đơn hàng ZaloPay: {ex.Message}");
            }
        }

        // Handle ZaloPay callback
        [HttpPost("zalo-callback")]
        public async Task<IActionResult> ZaloPayCallback()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            var callbackData = JsonConvert.DeserializeObject<Dictionary<string, string>>(body);
            if (callbackData == null || !callbackData.ContainsKey("data") || !callbackData.ContainsKey("mac"))
            {
                return BadRequest("Invalid callback data");
            }

            var data = callbackData["data"];
            var status = callbackData["type"];
            var mac = callbackData["mac"];

            // TODO: Kiểm tra MAC nếu muốn đảm bảo an toàn
            // var macLocal = GenerateMacForCallback(data);
            // if (mac != macLocal)
            // {
            //     return BadRequest("Invalid MAC");
            // }

            try
            {
                var jsonData = JObject.Parse(data);
                var appTransId = jsonData["app_trans_id"]?.ToString();
                var embedDataStr = jsonData["embed_data"]?.ToString();

                if (string.IsNullOrEmpty(embedDataStr))
                {
                    return BadRequest("Missing embed_data");
                }

                var embedData = JsonConvert.DeserializeObject<JObject>(embedDataStr);

                // 👉 Parse lại DonHangDto từ embed_data["order"]
                var orderToken = embedData["order"];
                if (orderToken == null)
                {
                    return BadRequest("Missing order data in embed_data");
                }

                var order = orderToken.ToObject<DonHangDto>();
                var lichSuGiaoDichId = embedData["lichSuGiaoDichId"]?.ToObject<Guid?>();
                var accountId = embedData["account_id"]?.ToObject<Guid?>();

                if (status == "1")
                {
                    Console.WriteLine($"✅ Thanh toán thành công - Đơn hàng: {order.ma_don_hang}");

                    // 🔥 Gọi lưu đơn hàng
                    await _zaloPaymentService.CreateOrder(order, lichSuGiaoDichId.Value, accountId.Value, 1);
                }
                else
                {
                    Console.WriteLine($"❌ Thanh toán thất bại - Đơn hàng: {order.ma_don_hang}");

                    // 🔥 Gọi cập nhật trạng thái thất bại nếu cần
                    await _zaloPaymentService.CreateOrder(order, lichSuGiaoDichId.Value, accountId.Value, 0);
                }

                // Trả về OK để ZaloPay không retry
                return Ok(new { return_code = 1, return_message = "OK" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Callback exception: {ex.Message}");
                return BadRequest("Error processing callback");
            }
        }



        [HttpPost("create-checkout-session")]
        public ActionResult CreateCheckoutSession([FromBody]PaymentParam req)
        {
            try
            {
                var session = _paymentService.CreateCheckoutSession(req);
                var theLastRecord = _context.lich_su_giao_dich.Where(x=> x.status == "Success").OrderByDescending(p => p.Created).FirstOrDefault();
                var newGiaoDich = new lich_su_giao_dich
                {
                    id = Guid.NewGuid(),
                    stripeSessionId = session.Id,
                    status = "Pending",
                    Created = DateTime.Now,
                    CreatedBy = req.userId,
                    giao_dich = req.priceInCents,
                    ngay_giao_dich = DateTime.Now,
                    loai_giao_dich = 1, // 1-doanh thu đơn hàng
                    phuong_thuc_giao_dich = 0, //0-stripe
                    so_du = theLastRecord != null ? (theLastRecord.so_du != null ? theLastRecord.so_du : 0) + (req.priceInCents ?? 0) : (req.priceInCents ?? 0),
                };
                // Record the session in your DB
                _context.lich_su_giao_dich.Add(newGiaoDich);
                _context.SaveChanges();
                return Ok(new { sessionId = session.Id });
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("success")]
        public async Task<IActionResult> Success([FromBody] PaymentParam param)
        {
            try
            {
                var paymentRecord = await _context.lich_su_giao_dich.FirstOrDefaultAsync(p => p.stripeSessionId == param.stripeSessionId);
                if (paymentRecord != null)
                {
                    var DonHang = param.donHang;
                    //xử lý đơn hàng
                    var newDonHang = new don_hang
                    {
                        id = Guid.NewGuid(),
                        ma_don_hang = GenerateOrderId(),
                        account_id = Guid.Parse(param.userId!),
                        trang_thai = 1,
                        Created = DateTime.Now,
                        dia_chi = DonHang!.tai_khoan!.dia_chi,
                        dvvc_id = GetRandomDvvcId(),
                        so_dien_thoai = DonHang!.tai_khoan!.so_dien_thoai,
                        ngay_mua = DateTime.Now,
                        tong_tien = param.priceInCents ?? 0,
                        thanh_tien = param.priceInCents ?? 0,
                    };
                    _context.don_hang.Add(newDonHang);

                    var chiTietDonHang = new List<chi_tiet_don_hang>();
                    DonHang.ds_chi_tiet_don_hang!.ForEach(x =>
                    {
                        var newCT = new chi_tiet_don_hang
                        {
                            id = Guid.NewGuid(),
                            don_hang_id = newDonHang.id,
                            san_pham_id = x.san_pham_id ?? Guid.NewGuid(),
                            Created = DateTime.Now,
                            don_gia = x.don_gia,
                            so_luong = x.so_luong,
                            thanh_tien = x.thanh_tien ?? 0,
                            LastModified = DateTime.Now,
                        };
                        chiTietDonHang.Add(newCT);
                    });
                    _context.chi_tiet_don_hang.AddRange(chiTietDonHang);
                    // Xóa sản phẩm khỏi giỏ hàng
                    var cartItems = await _context.gio_hang.FirstOrDefaultAsync(c => c.account_id == Guid.Parse(param.userId!));

                    if (cartItems != null)
                    {
                        var cartItemDetail = _context.chi_tiet_gio_hang
                        .Where(c => c.gio_hang_id == cartItems!.id)
                        .ToList();
                        // Nếu bạn muốn xóa chỉ những sản phẩm đã mua, bạn cần lọc dựa trên DonHang.ds_chi_tiet_don_hang
                        var CTIds = DonHang.ds_chi_tiet_don_hang!.Select(x => x.san_pham_id);
                        foreach (var item in cartItemDetail)
                        {
                            var productToRv = cartItemDetail.Where(x => CTIds.Contains(x.san_pham_id));
                            _context.chi_tiet_gio_hang.RemoveRange(productToRv);
                        }
                    }

                    paymentRecord.status = "Success";
                    await _context.SaveChangesAsync();
                }

                // Redirect to a success page or return success response
                return Ok("Payment successful.");
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the payment process
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("cancel")]
        public async Task<IActionResult> Cancel(string sessionId)
        {
            var paymentRecord = await _context.lich_su_giao_dich.FirstOrDefaultAsync(p => p.stripeSessionId == sessionId);
            if (paymentRecord != null)
            {
                paymentRecord.status = "Cancelled";
                await _context.SaveChangesAsync();
            }

            // Redirect to a cancel page or return cancel response
            return Ok("Payment cancelled.");
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

        private Guid GetRandomDvvcId() // Thay YourDbContext bằng DbContext của bạn
        {
            var dvvcs = _context.dvvc.ToList(); // Lấy tất cả các bản ghi dvvc

            if (dvvcs.Count == 0)
            {
                // Xử lý trường hợp bảng dvvc rỗng (ví dụ: trả về Guid.Empty hoặc ném ngoại lệ)
                return Guid.Empty; // Hoặc throw new Exception("Bảng dvvc rỗng.");
            }

            var random = new Random();
            var randomIndex = random.Next(0, dvvcs.Count); // Tạo chỉ số ngẫu nhiên

            return dvvcs[randomIndex].id; // Trả về ID của bản ghi ngẫu nhiên
        }
    }
}

