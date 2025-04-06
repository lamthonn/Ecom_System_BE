using Ecom.Interfaces;
using Ecom.Entity;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ecom.Dto.VanHanh;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using Ecom.Context;
using Microsoft.Identity.Client;

namespace Ecom.Services
{
    public class ZaloPaymentService : IZaloPaymentService
    {
        private const string ZaloPayAppId = "2553";
        private const string ZaloPayAppKey = "PcY4iZIKFCIdgZvA6ueMcMHHUbRLYjPL";
        private const string ZaloPayKey1 = "kLtgPl8HHhfvMuDHPwKfgfsY4Ydm9eIz";
        private const string ClientUrl = "http://localhost:4000";
        private const string CallbackUrl = "https://2bf5-42-115-143-34.ngrok-free.app/api/thanh-toan/zalo-callback";

        private readonly HttpClient _httpClient;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ZaloPaymentService(HttpClient httpClient, AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<JObject> CreateZaloPayOrderAsync(DonHangDto order)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                throw new Exception("Không thể truy xuất HttpContext");

            var userIdClaim = httpContext.User.FindFirst("id");
            if (userIdClaim == null)
                throw new Exception("Không tìm thấy ID người dùng trong token");

            var userId = Guid.Parse(userIdClaim.Value);
            var user = await _context.account.FirstOrDefaultAsync(x => x.id == userId);
            // tạo đơn hàng trạng thái pending
            var lichSuGiaoDichId = Guid.NewGuid();
            var theLastRecord = _context.lich_su_giao_dich.Where(x => x.status == "Success").OrderByDescending(p => p.Created).FirstOrDefault();
            var newGiaoDich = new lich_su_giao_dich
            {
                id = lichSuGiaoDichId,
                status = "Pending",
                Created = DateTime.Now,
                CreatedBy = userIdClaim.Value,
                giao_dich = order.thanh_tien,
                ngay_giao_dich = DateTime.Now,
                loai_giao_dich = 1, // 1-doanh thu đơn hàng
                phuong_thuc_giao_dich = 0, //0-stripe
                so_du = theLastRecord != null ? (theLastRecord.so_du != null ? theLastRecord.so_du : 0) + (order.thanh_tien ?? 0) : (order.thanh_tien ?? 0),
            };
            // Record the session in your DB
            _context.lich_su_giao_dich.Add(newGiaoDich);
            _context.SaveChanges();

            var rnd = new Random();
            var transID = rnd.Next(1000000); // random order ID

            var app_trans_id = DateTime.Now.ToString("yyMMdd") + "_" + transID;
            var app_time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var embed_data = new { 
                redirecturl = ClientUrl,
                order = order,
                lichSuGiaoDichId = lichSuGiaoDichId,
                account_id = userId
            };
            var items = new[]
            {
                new
                {
                    itemid = "product123",
                    itemname = "Sản phẩm mẫu",
                    itemprice = order.tong_tien,
                    itemquantity = 1
                }
            };

            var param = new Dictionary<string, string>
            {
                { "app_id", ZaloPayAppId },
                { "app_user", order.account_id.ToString() },
                { "app_time", app_time.ToString() },
                { "amount", ((long)(order.thanh_tien ?? 0)).ToString() },
                { "app_trans_id", app_trans_id },
                { "embed_data", JsonConvert.SerializeObject(embed_data) },
                { "item", JsonConvert.SerializeObject(items) },
                { "description", $"Thanh toán đơn hàng #{order.ma_don_hang}" },
                { "bank_code", "zalopayapp" },
                { "callback_url", CallbackUrl }
            };

            // Dữ liệu dùng để tạo MAC
            var data = $"{ZaloPayAppId}|{app_trans_id}|{param["app_user"]}|{param["amount"]}|{param["app_time"]}|{param["embed_data"]}|{param["item"]}";
            param.Add("mac", GenerateMac(data));

            // Gửi request tạo đơn
            var content = new FormUrlEncodedContent(param);

            try
            {
                var response = await _httpClient.PostAsync("https://sb-openapi.zalopay.vn/v2/create", content);
                var responseData = await response.Content.ReadAsStringAsync();

                var jsonResponse = JObject.Parse(responseData); // Trả về dạng object

                if (response.IsSuccessStatusCode)
                {
                    return jsonResponse;
                }
                else
                {
                    throw new Exception($"ZaloPay API Error: {responseData}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when calling ZaloPay API: {ex.Message}");
            }
        }

        private string GenerateMac(string data)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(ZaloPayAppKey)))
            {
                var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

      
        public async Task<string> HandleZaloPayCallbackAsync(string callbackData)
        {
            // Handle ZaloPay callback and update order status in the database
            // You can parse callbackData and verify payment status here
            return "Payment received successfully!";
        }

        public async Task<bool> CreateOrder(DonHangDto order, Guid? lichSuGiaoDichId, Guid? accountId, int? trang_thai)
        {
            try
            {
                if(trang_thai.HasValue && trang_thai.Value != 1 && lichSuGiaoDichId.HasValue)
                {
                    var lsurecored = await _context.lich_su_giao_dich.FirstOrDefaultAsync(p => p.id == lichSuGiaoDichId);
                    if (lsurecored != null) lsurecored.status = "Cancelled";
                    _context.lich_su_giao_dich.Update(lsurecored);
                }
                else
                {
                    var userId = new Guid();
                    if(lichSuGiaoDichId.HasValue)
                    {
                        var lsurecored = await _context.lich_su_giao_dich.FirstOrDefaultAsync(p => p.id == lichSuGiaoDichId);
                        if (lsurecored != null)
                        {
                            lsurecored.status = "Success";
                        }
                    }
                    else
                    {
                        var httpContext = _httpContextAccessor.HttpContext;
                        if (httpContext == null)
                            throw new Exception("Không thể truy xuất HttpContext");

                        var userIdClaim = httpContext.User.FindFirst("id");
                        if (userIdClaim == null)
                            throw new Exception("Không tìm thấy ID người dùng trong token");

                        userId = Guid.Parse(userIdClaim.Value);
                    }
                    var DonHang = order;
                    //xử lý đơn hàng
                    var newDonHang = new don_hang
                    {
                        id = Guid.NewGuid(),
                        ma_don_hang = GenerateOrderId(),
                        account_id = accountId ?? userId,
                        trang_thai = 1,
                        Created = DateTime.Now,
                        dia_chi = DonHang!.tai_khoan!.dia_chi,
                        dvvc_id = GetRandomDvvcId(),
                        so_dien_thoai = DonHang!.tai_khoan!.so_dien_thoai,
                        ngay_mua = DateTime.Now,
                        tong_tien = DonHang.tong_tien ?? 0,
                        thanh_tien = DonHang.thanh_tien ?? 0,
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
                    var cartItems = await _context.gio_hang.FirstOrDefaultAsync(c => c.account_id == accountId);

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
                  
                          
                }
                await _context.SaveChangesAsync(new CancellationToken());
                return true;
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the payment process
                throw new Exception("looic: " + ex.Message.ToString());
            }
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
    public class ZaloPayOrderRequest
    {
        public string app_id { get; set; }
        public string app_trans_id { get; set; }
        public string app_user { get; set; }
        public long amount { get; set; }
        public long app_time { get; set; }
        public long expire_duration_seconds { get; set; }
        public string item { get; set; }
        public string description { get; set; }
        public string embed_data { get; set; }
        public string bank_code { get; set; }
        public string mac { get; set; }
        public string callback_url { get; set; }
        public string device_info { get; set; }
        public string sub_app_id { get; set; }
        public string title { get; set; }
        public string currency { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
    }


}
