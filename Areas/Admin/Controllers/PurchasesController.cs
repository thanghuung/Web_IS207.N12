using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WEB2.Areas.Order;
using WEB2.Data;
using WEB2.Models;

namespace WEB2.Areas.Admin.Controllers {

    [Area("Admin")]
    public class PurchasesController : Controller {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IOptions<MyConfig> _config;

        public PurchasesController(AppDbContext context,
             UserManager<AppUser> userManager,
             IOptions<MyConfig> config) {
            _context = context;
            _userManager = userManager;
            _config = config;
        }

        public async Task<ActionResult> Payment(int id) {
            //string url = ConfigurationManager.AppSettings["Url"];
            //string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            //string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            //string hashSecret = ConfigurationManager.AppSettings["HashSecret"];

            string url = _config.Value.Url;
            string returnUrl = _config.Value.Returnurlpur;
            string tmnCode = _config.Value.TmnCode;
            string hashSecret = _config.Value.HashSecret;
            PayLib pay = new();

            var pur = await _context.Purchase.FindAsync(id);
            double money = pur.Paid * 100;

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
            pay.AddRequestData("vnp_TxnRef", pur.PurchaseId.ToString()); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);
            return Redirect(paymentUrl);
        }

        public async Task<ActionResult> PaymentConfirm() {
            if (HttpContext.Request.Query.Count > 0) {
                string hashSecret = _config.Value.HashSecret;
                var vnpayData = Request.Query;

                PayLib pay = new PayLib();
                // lấy toàn bộ dữ liệu được trả về
                foreach (var s in vnpayData) {
                    pay.AddResponseData(s.Key, s.Value);
                }

                int purId = Convert.ToInt32(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = HttpContext.Request.Query["vnp_SecureHash"]; //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?
                var pur = await _context.Purchase.FindAsync(purId);
                if (checkSignature) {
                    if (vnp_ResponseCode == "00") {
                        //Thanh toán thành công
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + purId + " | Mã giao dịch: " + vnpayTranId;

                        pur.TransactStatus = "done";
                        pur.ResponseCode = vnp_ResponseCode;
                        pur.SecureHash = vnp_SecureHash;
                        pur.PaymentDate = DateTime.Now;
                    } else {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + purId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                    }
                } else {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }

                pur.TransactionNo = vnpayTranId.ToString();

                //cap nhat order
                _context.Update(pur);
                await _context.SaveChangesAsync();
                //cap nhat tinh trang cua chi tiet hoa don
                var purdetail = await _context.PurchaseDetail.Where(o => o.PurchaseId == purId).ToListAsync();

                foreach (var item in purdetail) {
                    item.Status = "received";
                    //update so luong san pham trong kho
                    var inv = await _context.Invent_Product.Where(p => p.InventoryId == 1).Where(p => p.ProductId == item.ProductId).FirstOrDefaultAsync();
                    inv.ProductAvailable += item.Quantity;
                    _context.Update(inv);
                    await _context.SaveChangesAsync();
                    //update giá gốc của sản phẩm
                    var pro = await _context.Product.FindAsync(item.ProductId);
                    pro.RawPrice = item.Price;
                    _context.Update(pro);
                    await _context.SaveChangesAsync();

                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
            }

            return View();
        }
    }
}