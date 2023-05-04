using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WEB2.Data;
using WEB2.Models;
using System.Configuration;
using WEB2.Areas.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace WEB2.Controllers {

    [Authorize]
    public class OrdersController : Controller {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IOptions<MyConfig> _config;
        private readonly IEmailSender _emailSender;

        public OrdersController(
            AppDbContext context,
             UserManager<AppUser> userManager,
             IOptions<MyConfig> config,
             IEmailSender emailSender) {
            _context = context;
            _userManager = userManager;
            _config = config;
            _emailSender = emailSender;
        }

        // GET: Orders
        [EnableCors]
        public IActionResult Index(int id) {
            return View();
        }

        // GET: Orders/Details/5

        // GET: Orders/Edit/5

        public async Task<ActionResult> Payment(int id) {
            //string url = ConfigurationManager.AppSettings["Url"];
            //string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            //string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            //string hashSecret = ConfigurationManager.AppSettings["HashSecret"];

            string url = _config.Value.Url;
            string returnUrl = _config.Value.Returnurl;
            string tmnCode = _config.Value.TmnCode;
            string hashSecret = _config.Value.HashSecret;
            PayLib pay = new();

            var order = await _context.Order.FindAsync(id);
            double money = order.Paid * 100;

            pay.AddRequestData("vnp_Version", "2.0.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.0.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", money.ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);
            return Redirect(paymentUrl);
        }

        public async Task<ActionResult> PaymentConfirm() {
            if (HttpContext.Request.Query.Count > 0) {
                string hashSecret = _config.Value.HashSecret;
                var vnpayData = Request.Query;

                PayLib pay = new PayLib();
                Console.WriteLine(vnpayData);
                // lấy toàn bộ dữ liệu được trả về
                foreach (var s in vnpayData) {
                    pay.AddResponseData(s.Key, s.Value);
                }

                int orderId = Convert.ToInt32(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = HttpContext.Request.Query["vnp_SecureHash"]; //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?
                var order = await _context.Order.FindAsync(orderId);
                if (checkSignature) {
                    if (vnp_ResponseCode == "00") {
                        //Thanh toán thành công
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;

                        order.OTP = "123456";

                        order.TransactStatus = "paid";
                        order.ResponseCode = vnp_ResponseCode;
                        order.SecureHash = vnp_SecureHash;
                        order.PaymentDate = DateTime.Now;
                    } else {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                        order.Errlog = vnp_ResponseCode;
                        return View();
                    }
                } else {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }

                order.TransactionNo = vnpayTranId.ToString();

                //cap nhat order
                _context.Update(order);
                await _context.SaveChangesAsync();
                //cap nhat tinh trang cua gio hang
                var orderdetail = await _context.OrderDetail.Where(o => o.OrderId == orderId).ToListAsync();

                foreach (var item in orderdetail) {
                    item.Status = "solved";
                    //update so luong san pham
                    var product = await _context.Product.Where(p => p.ProductId == item.ProductId).FirstAsync();
                    //available here
                    product.CurrentOrder += item.Quantity;
                    product.UnitInOrder += 1;
                    product.Sold += item.Quantity;

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }

                var user = await _userManager.GetUserAsync(User);

                // phát sinh token theo thông tin user để xác nhận email mỗi user dựa vào thông tin
                // sẽ có một mã riêng, mã này nhúng vào link trong email gửi đi để người dùng xác nhận

                var callbackUrl = Url.Page(
                    "/Account/Manage/Bill",
                    pageHandler: null,
                    values: new { area = "Identity" },
                    protocol: Request.Scheme);
                // Gửi email
                await _emailSender.SendEmailAsync(
                    user.Email,
                    "Cảm ơn bạn đã mua hàng trên TechN",
                    $"Đơn hàng của bạn:{orderId}\nNgày đặt hàng: {order.OrderDay}\n<a href='{callbackUrl}'>Bấm vào đây để xem chi tiết</a>.");
            }

            return View();
        }
    }
}