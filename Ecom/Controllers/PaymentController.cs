using Ecom.Context;
using Ecom.Dto;
using Ecom.Entity;
using Ecom.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Ecom.Controllers.common
{
    [Route("api/thanh-toan")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly StripePaymentService _paymentService;
        private readonly AppDbContext _context;

        public PaymentController(StripePaymentService paymentService, AppDbContext context)
        {
            _paymentService = paymentService;
            _context = context;
        }




        [HttpPost("create-checkout-session")]
        public ActionResult CreateCheckoutSession([FromBody]PaymentParam req)
        {
            try
            {
                var session = _paymentService.CreateCheckoutSession(req);
                var theLastRecord = _context.lich_su_giao_dich.OrderByDescending(p => p.Created).FirstOrDefault();
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

                var  chiTietDonHang = new List<chi_tiet_don_hang>();
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

                paymentRecord.status = "Success";
                await _context.SaveChangesAsync();
            }

            // Redirect to a success page or return success response
            return Ok("Payment successful.");
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

