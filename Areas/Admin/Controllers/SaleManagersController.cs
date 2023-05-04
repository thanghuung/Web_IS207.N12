using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2.Data;
using WEB2.Models;



using System;


using System.Data;
using OfficeOpenXml;
using System.IO;

using System.Drawing;
using OfficeOpenXml.Style;


namespace WEB2.Areas.Admin.Controllers {

    [Area("Admin")]
    [Authorize("Staff")]
    public class SaleManagersController : Controller {
        private readonly AppDbContext _context;

        public SaleManagersController(AppDbContext context) {
            _context = context;
        }

        // GET: SaleManager
        public async Task<ActionResult> Transaction() {
            var order = await _context.OrderDetail
             .Include(o => o.Order)
             .Include(o => o.Order.Customer)
             .Include(o => o.Order.Customer.AppUser)
             .Include(o => o.Product)
             .Where(p => p.Order.TransactStatus != "null")
             .Where(p => p.Order.TransactStatus != "pending")
             .Where(p => p.Status == "solved")
             .ToListAsync();

            if (order.Count == 0) {
                return View(order);
            }
            OrderDetail od = new OrderDetail();
            var reorder = new List<OrderDetail>();
            bool check = false;

            for (int i = 0 ; i < order.Count - 1 ; i++) {
                if (check == false) {
                    od = order[i];
                    od.Voucher = od.Product.ProductName;
                    od.IDSKU = od.Quantity.ToString();
                }

                if (order[i].OrderId == order[i + 1].OrderId) {
                    od.Voucher += "\n" + order[i + 1].Product.ProductName;
                    od.IDSKU += "\n" + order[i + 1].Quantity.ToString();
                    check = true;
                } else {
                    reorder.Add(od);
                    check = false;
                }
            }
            if (check == false) {
                od = order[order.Count - 1];
                od.Voucher = od.Product.ProductName;
                od.IDSKU = od.Quantity.ToString();
            }
            reorder.Add(od);

            return View(reorder);
        }

        // GET: SaleManager
        public async Task<ActionResult> ProductBill() {
            var order = await _context.OrderDetail
              .Include(o => o.Order)
              .Include(o => o.Order.Customer)
              .Include(o => o.Order.Customer.AppUser)
              .Include(o => o.Product)
              .Where(p => p.Order.TransactStatus != "null")
              .Where(p => p.Order.TransactStatus != "pending")
              .Where(p => p.Order.TransactStatus != "done")
              .Where(p => p.Order.TransactStatus != "cancel")
              .Where(p => p.Order.TransactStatus != "shipping")
               .Where(p => p.Order.TransactStatus != "accept")
              .Where(p => p.Status == "solved")
              .ToListAsync();
            if (order.Count == 0) {
                return View(order);
            }
            OrderDetail od = new OrderDetail();
            var reorder = new List<OrderDetail>();
            bool check = false;

            for (int i = 0 ; i < order.Count - 1 ; i++) {
                if (check == false) {
                    od = order[i];
                    od.Voucher = od.Product.ProductName;
                    od.IDSKU = od.Quantity.ToString();
                }

                if (order[i].OrderId == order[i + 1].OrderId) {
                    od.Voucher += "\n" + order[i + 1].Product.ProductName;
                    od.IDSKU += "\n" + order[i + 1].Quantity.ToString();
                    check = true;
                } else {
                    reorder.Add(od);
                    check = false;
                }
            }
            if (check == false) {
                od = order[order.Count - 1];
                od.Voucher = od.Product.ProductName;
                od.IDSKU = od.Quantity.ToString();
            }
            reorder.Add(od);

            return View(reorder);
        }

        public async Task<IActionResult> Shipping() {
            var order = await _context.OrderDetail
           .Include(o => o.Order)
           .Include(o => o.Order.Customer)
           .Include(o => o.Order.Customer.AppUser)
           .Include(o => o.Product)
           .Where(p => p.Order.TransactStatus == "shipping")
           .Where(p => p.Status == "solved")
           .ToListAsync();

            if (order.Count == 0) {
                return View(order);
            }
            OrderDetail od = new OrderDetail();
            var reorder = new List<OrderDetail>();
            bool check = false;

            for (int i = 0 ; i < order.Count - 1 ; i++) {
                if (check == false) {
                    od = order[i];
                    od.Voucher = od.Product.ProductName;
                    od.IDSKU = od.Quantity.ToString();
                }

                if (order[i].OrderId == order[i + 1].OrderId) {
                    od.Voucher += "\n" + order[i + 1].Product.ProductName;
                    od.IDSKU += "\n" + order[i + 1].Quantity.ToString();
                    check = true;
                } else {
                    reorder.Add(od);
                    check = false;
                }
            }
            if (check == false) {
                od = order[order.Count - 1];
                od.Voucher = od.Product.ProductName;
                od.IDSKU = od.Quantity.ToString();
            }
            reorder.Add(od);

            return View(reorder);
        }

        public async Task<IActionResult> Done() {
            var order = await _context.OrderDetail
           .Include(o => o.Order)
           .Include(o => o.Order.Customer)
           .Include(o => o.Order.Customer.AppUser)
           .Include(o => o.Product)
           .Where(p => p.Order.TransactStatus == "done")
           .Where(p => p.Status == "solved")
           .ToListAsync();

            if (order.Count == 0) {
                return View(order);
            }
            OrderDetail od = new OrderDetail();
            var reorder = new List<OrderDetail>();
            bool check = false;

            for (int i = 0 ; i < order.Count - 1 ; i++) {
                if (check == false) {
                    od = order[i];
                    od.Voucher = od.Product.ProductName;
                    od.IDSKU = od.Quantity.ToString();
                }

                if (order[i].OrderId == order[i + 1].OrderId) {
                    od.Voucher += "\n" + order[i + 1].Product.ProductName;
                    od.IDSKU += "\n" + order[i + 1].Quantity.ToString();
                    check = true;
                } else {
                    reorder.Add(od);
                    check = false;
                }
            }
            if (check == false) {
                od = order[order.Count - 1];
                od.Voucher = od.Product.ProductName;
                od.IDSKU = od.Quantity.ToString();
            }
            reorder.Add(od);

            return View(reorder);
        }

        public async Task<IActionResult> Cancel() {
            var order = await _context.OrderDetail
           .Include(o => o.Order)
           .Include(o => o.Order.Customer)
           .Include(o => o.Order.Customer.AppUser)
           .Include(o => o.Product)
           .Where(p => p.Order.TransactStatus == "cancel")
           .Where(p => p.Status == "solved")
           .ToListAsync();

            if (order.Count == 0) {
                return View(order);
            }
            OrderDetail od = new OrderDetail();
            var reorder = new List<OrderDetail>();
            bool check = false;

            for (int i = 0 ; i < order.Count - 1 ; i++) {
                if (check == false) {
                    od = order[i];
                    od.Voucher = od.Product.ProductName;
                    od.IDSKU = od.Quantity.ToString();
                }

                if (order[i].OrderId == order[i + 1].OrderId) {
                    od.Voucher += "\n" + order[i + 1].Product.ProductName;
                    od.IDSKU += "\n" + order[i + 1].Quantity.ToString();
                    check = true;
                } else {
                    reorder.Add(od);
                    check = false;
                }
            }
            if (check == false) {
                od = order[order.Count - 1];
                od.Voucher = od.Product.ProductName;
                od.IDSKU = od.Quantity.ToString();
            }
            reorder.Add(od);

            return View(reorder);
        }

        // POST: SaleManager/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Accecpt(int id) {
        //    var order = await _context.Order.FindAsync(id);

        // if (order.TransactStatus.Equals("shipping")) order.TransactStatus = "done"; if
        // (order.TransactStatus.Equals("accept")) order.TransactStatus = "shipping"; if
        // (order.TransactStatus.Equals("pay by cash") || order.TransactStatus.Equals("paid"))
        // order.TransactStatus = "accept";

        //    _context.Update(order);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(ProductBill));
        //}
        /// <summary>
        /// transaction
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        public async Task<ActionResult> Accept_T(int id) {
            var order = await _context.Order.FindAsync(id);

            if (order.TransactStatus.Equals("shipping"))
                order.TransactStatus = "done";
            if (order.TransactStatus.Equals("accept")) {
                order.TransactStatus = "shipping";
                var or = await _context.Order.Where(p => p.OrderId == id).FirstOrDefaultAsync();
                var inven = await _context.Inventory.FindAsync(or.InventoryId);
                var ord = await _context.OrderDetail.Where(p => p.OrderId == id).ToListAsync();

                foreach (var item in ord) {
                    var inpro = await _context.Invent_Product.Where(p => p.ProductId == item.ProductId).Where(p => p.InventoryId == inven.InventoryId).FirstOrDefaultAsync();
                    inpro.ProductAvailable -= item.Quantity;

                    _context.Update(inpro);
                    await _context.SaveChangesAsync();
                }
            }

            if (order.TransactStatus.Equals("pay by cash") || order.TransactStatus.Equals("paid"))
                order.TransactStatus = "accept";

            _context.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Transaction));
        }

        public async Task<ActionResult> BillDetails_T(int id) {
            var order = await _context.OrderDetail
             .Include(o => o.Order)
             .Include(o => o.Order.Customer)
             .Include(o => o.Order.Customer.AppUser)
             .Include(o => o.Product)
             .Include(o => o.Order.Shipment)
             .Include(o => o.Order.Payment)
             .Where(o => o.Order.Deleted == false)
             .Where(p => p.OrderId == id)
             .ToListAsync();

            return View(order);
        }

        public async Task<ActionResult> BillDel_T(int id) {
            var order = await _context.Order.FindAsync(id);
            order.TransactStatus = "cancel";
            _context.Update(order);
            await _context.SaveChangesAsync();
            var orderdetail = await _context.OrderDetail.Where(p => p.OrderId == id).ToListAsync();
            foreach (var item in orderdetail) {
                item.Status = "";

                var pro = await _context.Product.Where(p => p.ProductId == item.ProductId).FirstAsync();
                pro.UnitInOrder -= 1;
                pro.CurrentOrder -= item.Quantity;
                _context.Update(pro);
                await _context.SaveChangesAsync();
                _context.Update(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Transaction));
        }

        /// <summary>
        /// shipping
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>

        public async Task<ActionResult> Accept_S(int id) {
            var order = await _context.Order.FindAsync(id);

            if (order.TransactStatus.Equals("shipping"))
                order.TransactStatus = "done";

            _context.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Shipping));
        }

        public async Task<ActionResult> BillDetails_S(int id) {
            var order = await _context.OrderDetail
             .Include(o => o.Order)
             .Include(o => o.Order.Customer)
             .Include(o => o.Order.Customer.AppUser)
             .Include(o => o.Product)
             .Include(o => o.Order.Shipment)
             .Include(o => o.Order.Payment)
             .Where(o => o.Order.Deleted == false)
             .Where(p => p.OrderId == id)
             .ToListAsync();

            return View(order);
        }

        public async Task<ActionResult> BillDel_S(int id) {
            var order = await _context.Order.FindAsync(id);
            order.TransactStatus = "cancel";
            _context.Update(order);
            await _context.SaveChangesAsync();
            var orderdetail = await _context.OrderDetail.Where(p => p.OrderId == id).ToListAsync();
            foreach (var item in orderdetail) {
                item.Status = "";

                var pro = await _context.Product.Where(p => p.ProductId == item.ProductId).FirstAsync();
                pro.UnitInOrder -= 1;
                pro.CurrentOrder -= item.Quantity;
                _context.Update(pro);
                await _context.SaveChangesAsync();
                _context.Update(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Shipping));
        }

        /// <summary>
        /// billdetail
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>

        public async Task<ActionResult> Accept_B(int id) {
            var order = await _context.Order.FindAsync(id);

            if (order.TransactStatus.Equals("accept")) {
                order.TransactStatus = "shipping";
                var or = await _context.Order.Where(p => p.OrderId == id).FirstOrDefaultAsync();
                var inven = await _context.Inventory.FindAsync(or.InventoryId);
                var ord = await _context.OrderDetail.Where(p => p.OrderId == id).ToListAsync();

                foreach (var item in ord) {
                    var inpro = await _context.Invent_Product.Where(p => p.ProductId == item.ProductId).Where(p => p.InventoryId == inven.InventoryId).FirstOrDefaultAsync();
                    inpro.ProductAvailable -= item.Quantity;

                    _context.Update(inpro);
                    await _context.SaveChangesAsync();
                }
            }

            if (order.TransactStatus.Equals("pay by cash") || order.TransactStatus.Equals("paid"))
                order.TransactStatus = "accept";

            _context.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ProductBill));
        }

        public async Task<ActionResult> BillDetails_B(int id) {
            var order = await _context.OrderDetail
             .Include(o => o.Order)
             .Include(o => o.Order.Customer)
             .Include(o => o.Order.Customer.AppUser)
             .Include(o => o.Product)
             .Include(o => o.Order.Shipment)
             .Include(o => o.Order.Payment)
             .Where(o => o.Order.Deleted == false)
             .Where(p => p.OrderId == id)
             .ToListAsync();

            return View(order);
        }

        public async Task<ActionResult> BillDel_B(int id) {
            var order = await _context.Order.FindAsync(id);
            order.TransactStatus = "cancel";
            _context.Update(order);
            await _context.SaveChangesAsync();
            var orderdetail = await _context.OrderDetail.Where(p => p.OrderId == id).ToListAsync();
            foreach (var item in orderdetail) {
                item.Status = "";

                var pro = await _context.Product.Where(p => p.ProductId == item.ProductId).FirstAsync();
                pro.UnitInOrder -= 1;
                pro.CurrentOrder -= item.Quantity;
                _context.Update(pro);
                await _context.SaveChangesAsync();
                _context.Update(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ProductBill));
        }

        public async Task<IActionResult> XuatFileAll() {
            // lenh khai bao
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var data = new DataTable();
            var stream = new MemoryStream();
            //int count;

            using (var package = new ExcelPackage(stream)) {
                // ten cua cai sheet
                var sheet = package.Workbook.Worksheets.Add("Danh sach các đơn bán hàng đã hoàn thành");
                //Font
                sheet.Cells["A1:H99"].Style.Font.Name = "Times New Roman";
                //merge cell lai
                sheet.Cells["A1:I1"].Merge = true;

                //chinh do rong cua cot
                sheet.Column(3).Width = 25;
                sheet.Column(1).Width = 5.33;
                sheet.Column(2).Width = 11.67;
                sheet.Column(5).Width = 20;
                //chu dong a1:c1
                sheet.Cells["A1:C1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["A1:C1"].Value = "CỬA HÀNG BÁN ĐỒ CÔNG NGHỆ TECHN";

                sheet.Cells["D6:E6"].Merge = true;
                sheet.Cells["F6:G6"].Merge = true;
                sheet.Cells["H6:I6"].Merge = true;
                sheet.Row(9).Height = 41.13;
                sheet.Cells["A6:I6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["A6:I6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells["A6:I6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells["A6:I6"].Style.Fill.BackgroundColor.SetColor(0, 186, 248, 255);
                sheet.Cells["A6:I6"].Style.Font.Bold = true;
                sheet.Cells["A6:I6"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                sheet.Cells["A6:I6"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                sheet.Cells["A6:I6"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                sheet.Cells["A6:I6"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                //value la ghi vo excel
                sheet.Cells["A6:A6"].Value = "STT";
                sheet.Cells["B6:B6"].Value = "Mã hóa đơn";
                sheet.Cells["C6:C6"].Value = "Tên khách hàng";
                sheet.Cells["D6:D6"].Value = "Tình trạng đơn hàng";
                sheet.Cells["F6:F6"].Value = "Số tiền";
                sheet.Cells["H6:H6"].Value = "Ngày đặt hàng";
                sheet.Name = "Danh sách đơn hàng";
                var query = await _context.Order.Include(p => p.Customer).Include(q => q.Customer.AppUser).ToListAsync();
                int x = 7;
                int y = 7;
                int z = 1;
                foreach (var p in query) {
                    string a = "A" + x + ":A" + y;
                    sheet.Cells[a].Value = z;
                    string b = "B" + x + ":B" + y;
                    sheet.Cells[b].Value = p.OrderId;
                    string c = "C" + x + ":C" + y;
                    sheet.Cells[c].Value = p.Customer.AppUser.FullName;

                    string e = "D" + x + ":E" + y;
                    sheet.Cells[e].Merge = true;

                    string d = "D" + x + ":D" + y;
                    if (p.TransactStatus == "done") {
                        sheet.Cells[d].Value = "Đã hoàn thành";
                    } else {
                        if (p.TransactStatus == "shipping") {
                            sheet.Cells[d].Value = "Đang vận chuyển";
                        } else {
                            if (p.TransactStatus == "paid") {
                                sheet.Cells[d].Value = "Đã thanh toán";
                            }
                        }
                    }

                    string g = "F" + x + ":G" + y;
                    sheet.Cells[g].Merge = true;

                    string f = "F" + x + ":F" + y;
                    sheet.Cells[f].Value = p.Paid;

                    string i = "H" + x + ":I" + y;
                    sheet.Cells[i].Merge = true;

                    string h = "H" + x + ":H" + y;
                    sheet.Cells[h].Style.Numberformat.Format = "dd/MM/yyyy";
                    sheet.Cells[h].Value = p.OrderDay;
                    x = x + 1;
                    y = y + 1;
                    z = z + 1;
                }

                package.Save();
            }
            stream.Position = 0;

            var tenfile = $"DS_BanHang_{DateTime.Now}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", tenfile);
        }

        public async Task<IActionResult> XuatFilePaid() {
            // lenh khai bao
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var data = new DataTable();
            var stream = new MemoryStream();
            /*int count*/;

            using (var package = new ExcelPackage(stream)) {
                // ten cua cai sheet
                var sheet = package.Workbook.Worksheets.Add("Danh sach các đơn bán hàng đã trả tiền");
                //Font
                sheet.Cells["A1:H99"].Style.Font.Name = "Times New Roman";
                //merge cell lai
                sheet.Cells["A1:I1"].Merge = true;

                //chinh do rong cua cot
                sheet.Column(3).Width = 25;
                sheet.Column(1).Width = 5.33;
                sheet.Column(2).Width = 11.67;
                sheet.Column(5).Width = 20;
                //chu dong a1:c1
                sheet.Cells["A1:C1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["A1:C1"].Value = "CỬA HÀNG BÁN ĐỒ CÔNG NGHỆ TECHN";

                sheet.Cells["D6:E6"].Merge = true;
                sheet.Cells["F6:G6"].Merge = true;
                sheet.Cells["H6:I6"].Merge = true;
                sheet.Row(9).Height = 41.13;
                sheet.Cells["A6:I6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["A6:I6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells["A6:I6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells["A6:I6"].Style.Fill.BackgroundColor.SetColor(0, 186, 248, 255);
                sheet.Cells["A6:I6"].Style.Font.Bold = true;
                sheet.Cells["A6:I6"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                sheet.Cells["A6:I6"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                sheet.Cells["A6:I6"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                sheet.Cells["A6:I6"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                //value la ghi vo excel
                sheet.Cells["A6:A6"].Value = "STT";
                sheet.Cells["B6:B6"].Value = "Mã hóa đơn";
                sheet.Cells["C6:C6"].Value = "Tên khách hàng";
                sheet.Cells["D6:D6"].Value = "Tình trạng đơn hàng";
                sheet.Cells["F6:F6"].Value = "Số tiền";
                sheet.Cells["H6:H6"].Value = "Ngày đặt hàng";
                sheet.Name = "Danh sách đơn hàng";
                var query = await _context.Order.Include(p => p.Customer).Include(q => q.Customer.AppUser)
                    .Where(k => k.TransactStatus == "paid").ToListAsync();
                int x = 7;
                int y = 7;
                int z = 1;
                foreach (var p in query) {
                    string a = "A" + x + ":A" + y;
                    sheet.Cells[a].Value = z;
                    string b = "B" + x + ":B" + y;
                    sheet.Cells[b].Value = p.OrderId;
                    string c = "C" + x + ":C" + y;
                    sheet.Cells[c].Value = p.Customer.AppUser.FullName;

                    string e = "D" + x + ":E" + y;
                    sheet.Cells[e].Merge = true;

                    string d = "D" + x + ":D" + y;
                    sheet.Cells[d].Value = "Đã thanh toán";

                    string g = "F" + x + ":G" + y;
                    sheet.Cells[g].Merge = true;

                    string f = "F" + x + ":F" + y;
                    sheet.Cells[f].Value = p.Paid;

                    string i = "H" + x + ":I" + y;
                    sheet.Cells[i].Merge = true;

                    string h = "H" + x + ":H" + y;
                    sheet.Cells[h].Style.Numberformat.Format = "dd/MM/yyyy";
                    sheet.Cells[h].Value = p.OrderDay;
                    x = x + 1;
                    y = y + 1;
                    z = z + 1;
                }

                package.Save();
            }
            stream.Position = 0;

            var tenfile = $"DS_BanHang_{DateTime.Now}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", tenfile);
        }

        public async Task<IActionResult> XuatFileShip() {
            // lenh khai bao
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var data = new DataTable();
            var stream = new MemoryStream();
            //int count;

            using (var package = new ExcelPackage(stream)) {
                // ten cua cai sheet
                var sheet = package.Workbook.Worksheets.Add("Danh sach các đơn bán hàng đang vận chuyển");
                //Font
                sheet.Cells["A1:H99"].Style.Font.Name = "Times New Roman";
                //merge cell lai
                sheet.Cells["A1:I1"].Merge = true;

                //chinh do rong cua cot
                sheet.Column(3).Width = 25;
                sheet.Column(1).Width = 5.33;
                sheet.Column(2).Width = 11.67;
                sheet.Column(5).Width = 20;
                //chu dong a1:c1
                sheet.Cells["A1:C1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["A1:C1"].Value = "CỬA HÀNG BÁN ĐỒ CÔNG NGHỆ TECHN";

                sheet.Cells["D6:E6"].Merge = true;
                sheet.Cells["F6:G6"].Merge = true;
                sheet.Cells["H6:I6"].Merge = true;
                sheet.Row(9).Height = 41.13;
                sheet.Cells["A6:I6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["A6:I6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells["A6:I6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells["A6:I6"].Style.Fill.BackgroundColor.SetColor(0, 186, 248, 255);
                sheet.Cells["A6:I6"].Style.Font.Bold = true;
                sheet.Cells["A6:I6"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                sheet.Cells["A6:I6"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                sheet.Cells["A6:I6"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                sheet.Cells["A6:I6"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                //value la ghi vo excel
                sheet.Cells["A6:A6"].Value = "STT";
                sheet.Cells["B6:B6"].Value = "Mã hóa đơn";
                sheet.Cells["C6:C6"].Value = "Tên khách hàng";
                sheet.Cells["D6:D6"].Value = "Tình trạng đơn hàng";
                sheet.Cells["F6:F6"].Value = "Số tiền";
                sheet.Cells["H6:H6"].Value = "Ngày đặt hàng";
                sheet.Name = "Danh sách đơn hàng";
                var query = await _context.Order.Include(p => p.Customer).Include(q => q.Customer.AppUser)
                    .Where(k => k.TransactStatus == "shipping").ToListAsync();
                int x = 7;
                int y = 7;
                int z = 1;
                foreach (var p in query) {
                    string a = "A" + x + ":A" + y;
                    sheet.Cells[a].Value = z;
                    string b = "B" + x + ":B" + y;
                    sheet.Cells[b].Value = p.OrderId;
                    string c = "C" + x + ":C" + y;
                    sheet.Cells[c].Value = p.Customer.AppUser.FullName;

                    string e = "D" + x + ":E" + y;
                    sheet.Cells[e].Merge = true;

                    string d = "D" + x + ":D" + y;
                    sheet.Cells[d].Value = "Đang vận chuyển";

                    string g = "F" + x + ":G" + y;
                    sheet.Cells[g].Merge = true;

                    string f = "F" + x + ":F" + y;
                    sheet.Cells[f].Value = p.Paid;

                    string i = "H" + x + ":I" + y;
                    sheet.Cells[i].Merge = true;

                    string h = "H" + x + ":H" + y;
                    sheet.Cells[h].Style.Numberformat.Format = "dd/MM/yyyy";
                    sheet.Cells[h].Value = p.OrderDay;
                    x = x + 1;
                    y = y + 1;
                    z = z + 1;
                }

                package.Save();
            }
            stream.Position = 0;

            var tenfile = $"DS_BanHang_{DateTime.Now}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", tenfile);
        }
    }
}