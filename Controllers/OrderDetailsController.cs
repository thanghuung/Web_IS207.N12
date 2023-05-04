using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WEB2.Areas.Order;
using WEB2.Data;
using WEB2.Models;

namespace WEB2.Controllers {

    [Authorize]
    public class OrderDetailsController : Controller {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IOptions<MyConfig> _config;
        private readonly IEmailSender _emailSender;

        public OrderDetailsController(
            AppDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IOptions<MyConfig> config,
            IEmailSender emailSender) {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _config = config;
            _emailSender = emailSender;
        }

        // GET: OrderDetails
        public async Task<IActionResult> AddCart(int productid) {
            var user = await _userManager.GetUserAsync(User);
            var userid = await _userManager.GetUserIdAsync(user);
            var customer = await _context.Customer.Where(o => o.UserId == userid).FirstAsync();
            if (user == null) {
                return NotFound($"Không tải được tài khoản ID = '{_userManager.GetUserId(User)}'.");
            }

            await AddtoCart(user, productid);

            // var appDbContext = _context.OrderDetail.Include(o => o.Order).Include(o => o.Product)
            // .Where(o => o.Order.CustomerId == customer.CustomerID);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] OrderDetail orderDetail) {
            var order = await _context.OrderDetail.Where(p => p.OrderId == orderDetail.OrderId)
                .Where(p => p.ProductId == orderDetail.ProductId).FirstOrDefaultAsync();
            var reorder = await _context.Order.FindAsync(orderDetail.OrderId);

            order.Quantity = orderDetail.Quantity;
            order.Total = orderDetail.Quantity * order.Price;
            var detail = await _context.OrderDetail.Where(p => p.OrderId == orderDetail.OrderId).ToListAsync();
            var max = 0.0;
            foreach (var item in detail) {
                max += item.Price * item.Quantity;
            }
            reorder.Paid = max;
            _context.Update(reorder);
            await _context.SaveChangesAsync();

            _context.Update(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index() {
            var user = await _userManager.GetUserAsync(User);
            var userid = await _userManager.GetUserIdAsync(user);
            var customer = await _context.Customer.Where(o => o.UserId == userid).FirstAsync();
            if (user == null) {
                return NotFound($"Không tải được tài khoản ID = '{_userManager.GetUserId(User)}'.");
            }
            var appDbContext = _context.OrderDetail.Include(o => o.Order).Include(o => o.Product)
            .Where(o => o.Order.TransactStatus != "paid")
            .Where(o => o.Order.TransactStatus != "done")
            .Where(o => o.Order.TransactStatus != "pay by cash")
             .Where(o => o.Order.TransactStatus != "shipping")
             .Where(o => o.Order.TransactStatus != "cancel")
               .Where(o => o.Order.TransactStatus != "accept")
            .Where(o => o.Order.CustomerId == customer.CustomerID);

            return View(await appDbContext.ToListAsync());
        }

        private async Task AddtoCart(AppUser user, int productid) {
            var userName = await _userManager.GetUserNameAsync(user);
            var userid = await _userManager.GetUserIdAsync(user);
            var customer = await _context.Customer.Where(o => o.UserId == userid).FirstAsync();

            var order = await _context.Order.Include(o => o.Customer)
                .Where(o => o.Customer.UserId == userid)
                .Where(o => o.TransactStatus != "paid")
                .Where(o => o.TransactStatus != "done")
                .Where(o => o.TransactStatus != "pay by cash")
                 .Where(o => o.TransactStatus != "shipping")
                 .Where(o => o.TransactStatus != "cancel")
                   .Where(o => o.TransactStatus != "accept")
                .FirstOrDefaultAsync();

            if (ModelState.IsValid) {
                var product = await _context.Product.FirstOrDefaultAsync(p => p.ProductId == productid);
                //nếu như khách hàng chưa có giỏ hàng nào thì tạo 1 hóa đơn để thêm giỏ hàng, còn nếu có rồi thì cứ việc thêm
                if (order == null) {
                    // nhập dữ liệu vào order
                    var neworder = new Order {
                        CustomerId = customer.CustomerID,
                        ShipperId = 1,
                        PaymentId = 1,
                        TransactStatus = "null",
                        Deleted = false,
                        Paid = product.UnitPrice
                    };

                    _context.Add(neworder);
                    await _context.SaveChangesAsync();
                    var reorder = await _context.Order.Include(o => o.Customer)
                          .Where(o => o.Customer.UserId == userid)
                          .Where(o => o.TransactStatus == "null").FirstOrDefaultAsync();
                    int orderid = reorder.OrderId;
                    //thêm vô giỏ hàng

                    var cart = new OrderDetail() {
                        OrderId = orderid,
                        ProductId = productid,
                        Quantity = 1,
                        Status = "saved",
                        Price = product.UnitPrice,

                        Fulfilled = false,
                        Total = product.UnitPrice,
                        Discount = 0,
                    };

                    _context.Add(cart);
                    await _context.SaveChangesAsync();
                } else {
                    int orderid = order.OrderId;
                    //nếu giỏ hàng chưa có sản phẩm này thì thêm, nếu có rồi thì tăng số lượng lên 1
                    var orderdetail = await _context.OrderDetail.Include(p => p.Order).
                        Where(p => p.OrderId == orderid)
                        .Where(p => p.Status == "saved")
                        .Where(p => p.Fulfilled == false)
                        .Where(p => p.ProductId == productid)
                        .ToListAsync();
                    if (orderdetail == null) {
                        return;
                    }
                    if (orderdetail.Count == 0) {
                        var cart = new OrderDetail() {
                            OrderId = orderid,
                            ProductId = productid,
                            Quantity = 1,
                            Status = "saved",
                            Price = product.UnitPrice,

                            Fulfilled = false,
                            Total = product.UnitPrice,
                            Discount = 0,
                        };
                        _context.Add(cart);
                        await _context.SaveChangesAsync();
                        order.Paid += cart.Total;
                        _context.Update(order);
                        await _context.SaveChangesAsync();
                    } else {
                    }
                }
            }
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? productid, int? orderid) {
            if (productid == null || orderid == null) {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail
                .Include(o => o.Order)
                .Include(o => o.Product)
                .Where(o => o.ProductId == productid)
               .FirstOrDefaultAsync(m => m.OrderId == orderid);

            if (orderDetail == null) {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int productid, int orderid) {
            var orderDetail = await _context.OrderDetail.Where(o => o.OrderId == orderid)
                .Where(o => o.ProductId == productid).FirstOrDefaultAsync();

            var order = await _context.Order.FindAsync(orderid);
            order.Paid -= orderDetail.Total;
            _context.Update(order);
            await _context.SaveChangesAsync();
            _context.OrderDetail.Remove(orderDetail);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(int id) {
            return _context.OrderDetail.Any(e => e.OrderId == id);
        }

        public async Task<IActionResult> Pay(int? id) {
            if (id == null) {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            var userid = await _userManager.GetUserIdAsync(user);

            var order = await _context.OrderDetail
                .Include(o => o.Order)
                .Include(o => o.Order.Customer)
                .Include(o => o.Order.Payment)
                .Include(o => o.Order.Shipment)
                .Include(o => o.Order.Customer)
                .Include(o => o.Order.Customer.AppUser)
                .Include(o => o.Product)
                .Include(o => o.Product.ProductDiscounts)
                .Where(o => o.OrderId == id)
                .Where(o => o.Order.OrderId == id)
                .Where(o => o.Order.Deleted == false)
                .Where(o => o.Status == "saved")
                .Where(o => o.Order.Customer.UserId == userid)
                .ToListAsync();

            var reorder = await _context.Order.Include(o => o.Customer).Include(o => o.Payment).Include(o => o.Shipment)
            .FirstOrDefaultAsync();

            // var voucher = await _context.Voucher_Details.Include(v => v.Customer).Include(v =>
            // v.Voucher) .Where(v => v.Customer.UserId == userid).FirstOrDefaultAsync();
            if (order == null) {
                return NotFound();
            }
            //var cus = await _context.Customer.Include(p => p.AppUser)
            //    .FirstOrDefaultAsync(p => p.UserId == userid);
            //double latpos = Convert.ToDouble(cus.Latitude);
            //double lonpos = Convert.ToDouble(cus.Longitude);
            //double max = 9999999999;
            //int invenid = 0;
            //// max : khoảng cách
            //var inven = await _context.Inventory.ToListAsync();
            //foreach (var item in inven) {
            //    double lat = Convert.ToDouble(item.Latitude);
            //    double lon = Convert.ToDouble(item.Longitude);
            //    double res = Math.Abs(Math.Sqrt(Math.Pow(latpos - lat, 2) + Math.Pow(lonpos - lon, 2)));
            //    if (res <= max) {
            //        max = res;
            //        invenid = item.InventoryId;
            //    }
            //}
            //var ordership = await _context.Order.FindAsync(id);
            /////tính tiền ship
            //max *= 100000;
            //ordership.InventoryId = invenid;
            //ordership.Freight = Math.Round(max, 2);
            //_context.Update(ordership);
            //await _context.SaveChangesAsync();

            // order[0].Order.Customer.ShipAddress = order[0].Order.Customer.ShipAddress + ", " +
            // order[0].Order.Customer.State + ", " + order[0].Order.Customer.City;
            ViewData["ShipperId"] = new SelectList(_context.Shipment, "ShipperId", "CompanyName", reorder.Shipment.CompanyName);
            ViewData["PaymentId"] = new SelectList(_context.Payment.Where(o => o.Allowed == true), "PaymentId", "PaymentType", reorder.Payment.PaymentType);
            // ViewData["VoucherId"] = new SelectList(_context.Voucher, "VoucherId", "VoucherName", voucher.Voucher.VoucherName);
            return View(order);
        }

        //public async Task<IActionResult> ShipMoney(int id) {
        //    var order = await _context.Order
        //        .Include(p => p.Customer)
        //        .Where(p => p.OrderId == id)
        //        .FirstOrDefaultAsync();
        //    return View(order);
        //}

        //[HttpPost]
        //public async Task<IActionResult> ShipAddress([FromBody] LatLong customer) {
        //    var cus = await _context.Customer.FindAsync(customer.CustomerID);
        //    cus.ShipAddress = customer.ShipAddress;
        //    cus.City = customer.City;
        //    cus.State = customer.State;
        //    _context.Update(cus);
        //    await _context.SaveChangesAsync();

        // if (cus.City.Equals("TP Hồ Chí Minh")) { switch (cus.State) { case "Quận 1": {
        // cus.Latitude = "10.7748455"; cus.Longitude = "106.6993497"; break; } case "Quận 12": {
        // cus.Latitude = "10.8672335"; cus.Longitude = "106.6539304"; break; } case "Quận Thủ Đức":
        // { cus.Latitude = "10.85142"; cus.Longitude = "106.74727"; break; } case "Quận 9": {
        // cus.Latitude = "10.5024"; cus.Longitude = "106.4615"; break; } case "Quận Gò Vấp": {
        // cus.Latitude = "10.84015"; cus.Longitude = "106.6710828"; break; } case "Quận Bình
        // Thạnh": { cus.Latitude = "10.8046591"; cus.Longitude = "106.7078477"; break; } case "Quận
        // Tân Bình": { cus.Latitude = "10.7979794"; cus.Longitude = "106.6538054"; break; }
        // default: break; } } // cus.Latitude = ""; cus.Longitude = ""; _context.Update(cus); await _context.SaveChangesAsync();

        //    return Json(new {
        //        newUrl = Url.Action("Pay", "OrderDetails", new { id = customer.OrderId })
        //    });
        //}

        [HttpPost("payment")]
        [EnableCors]
        public async Task<IActionResult> Payment([FromBody] ImageCoordinates coordinates) {
            var orderid = coordinates.Orderid;
            var shipaddress = coordinates.Shipaddress;
            var shipmoney = coordinates.Shippay;
            var paymoney = coordinates.Paid;
            var shipcom = coordinates.Shipcompany;
            var typepay = coordinates.TypePay;
            var provi = coordinates.Provi;
            //cap nhat dia chi giao hang
            var order = await _context.Order.FindAsync(orderid);
            var customer = await _context.Customer.FirstOrDefaultAsync(o => o.CustomerID == order.CustomerId);

            customer.ShipAddress = shipaddress;
            //cap nhat ship
            order.Freight = shipmoney;
            order.Paid = paymoney;
            //cap nhat ngay hoa don
            order.OrderDay = DateTime.Now;
            //cap nhat khoa ngoai
            order.ShipperId = shipcom;
            order.PaymentId = typepay;
            //cap nhat tinh trang hoa don
            order.TransactStatus = "pending";
            //cap nhat noi xuat kho
            if (provi.Equals("Hà Giang") ||
                provi.Equals("Cao Bằng") ||
                provi.Equals("Bắc Kạn") ||
                provi.Equals("Lạng Sơn") ||
                provi.Equals("Tuyên Quang") ||
                provi.Equals("Thái Nguyên") ||
                provi.Equals("Phú Thọ") ||
                provi.Equals("Bắc Giang") ||
                provi.Equals("Quảng Ninh") ||
                provi.Equals("Lào Cai") ||
                provi.Equals("Yên Bái") ||
                provi.Equals("Điện Biên") ||
                provi.Equals("Hoà Bình") ||
                provi.Equals("Lai Châu") ||
                provi.Equals("Sơn La") ||
                provi.Equals("Bắc Ninh") ||
                provi.Equals("Hà Nam") ||
                provi.Equals("Hà Nội") ||
                provi.Equals("Hải Dương") ||
                provi.Equals("Hải Phòng") ||
                provi.Equals("Hưng Yên") ||
                provi.Equals("Nam Định") ||
                provi.Equals("Ninh Bình") ||
                provi.Equals("Thái Bình") ||
                provi.Equals("Vĩnh Phúc") ||
                provi.Equals("Thanh Hoá") ||
                provi.Equals("Nghệ An") ||
                provi.Equals("Hà Tĩnh") ||
                provi.Equals("Quảng Bình") ||
                provi.Equals("Quảng Trị") ||
                provi.Equals("Thừa Thiên-Huế")) {
                order.InventoryId = 10;
            } else if (provi.Equals("Đà Nẵng") ||
                provi.Equals("Quảng Nam") ||
                provi.Equals("Quảng Ngãi") ||
                provi.Equals("Bình Định") ||
                provi.Equals("Phú Yên") ||
                provi.Equals("Khánh Hoà") ||
                provi.Equals("Ninh Thuận") ||
                provi.Equals("Bình Thuận") ||
                provi.Equals("Kon Tum") ||
                provi.Equals("Lâm Đồng") ||
                provi.Equals("Gia Lai") ||
                provi.Equals("Đắk Lắk") ||
                provi.Equals("Đắk Nông")) {
                order.InventoryId = 17;
            } else {
                order.InventoryId = 1;
            }

            //nếu trả bằng tiền mặt:
            if (typepay == 1) {
                order.TransactStatus = "pay by cash";
                var orderdetail = await _context.OrderDetail.Where(p => p.OrderId == orderid).Where(p => p.Status == "saved").ToListAsync();
                foreach (var item in orderdetail) {
                    item.Status = "solved";
                    var pro = await _context.Product.Where(p => p.ProductId == item.ProductId).FirstAsync();
                    pro.CurrentOrder += item.Quantity;
                    pro.UnitInOrder += 1;
                    pro.Sold += item.Quantity;
                    _context.Update(pro);
                    await _context.SaveChangesAsync();
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }

                _context.Update(customer);
                await _context.SaveChangesAsync();
                _context.Update(order);
                await _context.SaveChangesAsync();

                return Json(new {
                    newUrl = Url.Action("Thank", "OrderDetails")
                });
            }

            if (ModelState.IsValid) {
                _context.Update(customer);
                await _context.SaveChangesAsync();
                _context.Update(order);
                await _context.SaveChangesAsync();
            }

            return Json(new {
                newUrl = Url.Action("Payment", "Orders", new { id = orderid })
            });
        }

        public async Task<IActionResult> Thank() {
            var user = await _userManager.GetUserAsync(User);

            // phát sinh token theo thông tin user để xác nhận email mỗi user dựa vào thông tin sẽ
            // có một mã riêng, mã này nhúng vào link trong email gửi đi để người dùng xác nhận

            var order = await _context.OrderDetail
           .Include(o => o.Order)
           .Include(o => o.Product)
           .Where(p => p.Order.TransactStatus == "pay by cash")
           .Where(p => p.Status == "solved")
           .Where(p => p.Order.Customer.UserId == user.Id)
           .FirstAsync();

            var callbackUrl = Url.Page(
                "/Account/Manage/Bill",
                pageHandler: null,
                values: new { area = "Identity" },
                protocol: Request.Scheme);
            // Gửi email
            await _emailSender.SendEmailAsync(
                user.Email,
                "Cảm ơn bạn đã mua hàng trên TechN",
                $"Đơn hàng của bạn:{order.OrderId}\nNgày đặt hàng: {order.Order.OrderDay}\n<a href='{callbackUrl}'>Bấm vào đây để xem chi tiết</a>.");
            return View();
        }

        private bool CustomerExists(int id) {
            return _context.Customer.Any(e => e.CustomerID == id);
        }
    }
}