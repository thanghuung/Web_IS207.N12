using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB2.Areas.Order;
using WEB2.Data;
using WEB2.Models;

namespace WEB2.Areas.Admin.Controllers {

    [Area("Admin")]
    [Authorize("Staff")]
    public class DashboardsController : Controller {
        private readonly AppDbContext _context;

        public DashboardsController(AppDbContext context) {
            _context = context;
        }

        public IActionResult Dashboard() {
            return View();
        }

        public async Task<IActionResult> Dashboard1() {
            var order = await _context.OrderDetail
                .Include(o => o.Order)
                .Include(o => o.Order.Customer)
                .Include(o => o.Order.Customer.AppUser)
                .Include(o => o.Product)
                .Where(p => p.Order.TransactStatus != "null")
                .Where(p => p.Order.TransactStatus != "pending")
                .Where(p => p.Order.TransactStatus != "done")
                .Where(p => p.Order.TransactStatus != "cancel")
                .Where(p => p.Status == "solved")
                .ToListAsync();
            //if (order.Count < 1 || order == null) {
            //    return RedirectToAction(nameof(Dashboard));
            //}
            //OrderDetail od = new OrderDetail();
            //var reorder = new List<OrderDetail>();
            //bool check = false;

            //for (int i = 0 ; i < order.Count - 1 ; i++) {
            //    if (check == false) {
            //        od = order[i];
            //        od.IDSKU = od.Quantity.ToString();
            //    }

            //    if (order[i].OrderId == order[i + 1].OrderId) {
            //        od.Product.ProductName += "\n" + order[i + 1].Product.ProductName;
            //        od.IDSKU += "\n" + order[i + 1].IDSKU;
            //        check = true;
            //    } else {
            //        reorder.Add(od);
            //        check = false;
            //    }
            //}
            //if (check == false) {
            //    od = order[order.Count - 1];
            //    od.IDSKU = od.Quantity.ToString();
            //}

            //reorder.Add(od);
            //return View(reorder);
            double prof = 0;

            double today = 0;
            var profit = await _context.Order.ToListAsync();
            foreach (var item in profit) {
                prof += item.Paid;
            }
            var customer = await _context.Customer.ToListAsync();
            int cus = customer.Count();

            var tod = await _context.Order.Where(p => p.OrderDay.Date == DateTime.Now.Date)
                .Where(p => p.OrderDay.Month == DateTime.Now.Month)
                .Where(p => p.OrderDay.Year == DateTime.Now.Year)
                .ToListAsync();
            foreach (var item in tod) {
                today += item.Paid;
            }
            var feedback = await _context.Feedback.ToListAsync();
            int totalf = feedback.Count();

            var pos = await _context.Feedback.Where(p => p.Rate > 3).ToListAsync();
            int pos_f = pos.Count();
            var dis = await _context.Feedback.Where(p => p.Rate < 4).ToListAsync();
            int dis_f = dis.Count();

            var product = await _context.Product.OrderByDescending(p => p.Sold).Where(p => p.Sold >= 0).Where(p => p.View > 0).ToListAsync();
            // var product = await _context.Product.ToListAsync();
            int pro = product.Count();
            var products = new List<Product>();
            for (int i = 0 ; i < 10 ; i++) {
                products.Add(product[i]);
            }
            //List<int> v = new List<int>();
            //List<int> s = new List<int>();
            //List<string> n = new List<string>();

            //foreach (var item in products) {
            //    v.Add(item.Product.View);
            //    n.Add(item.Product.ProductName);
            //}

            var inven = await _context.Inventory.ToListAsync();
            var name = new List<string>();
            var cnt = new List<int>();

            foreach (var item in inven) {
                name.Add(item.Name);
                var inv_pro = await _context.Invent_Product.Where(P => P.InventoryId == item.InventoryId).ToListAsync();
                int countt = 0;
                foreach (var item2 in inv_pro) {
                    countt += item2.ProductAvailable;
                }
                cnt.Add(countt);
            }

            Db1 db1 = new Db1();
            db1.Profit = prof;
            db1.Customer = cus;
            db1.Product = pro;
            db1.Todei = today;

            db1.totalfeed = totalf;
            db1.pos_feed = pos_f;
            db1.dis_feed = dis_f;

            db1.orderdetails = order;
            db1.View = products;

            db1.inven_name = name;
            db1.inven_count = cnt;
            return View(db1);
        }

        public async Task<IActionResult> Dashboard2(int? year) {
            if (year == null)
                year = DateTime.Now.Year;
            double prof = 0;

            double today = 0;
            var profit = await _context.Order.ToListAsync();
            foreach (var item in profit) {
                prof += item.Paid;
            }
            var customer = await _context.Customer.ToListAsync();
            int cus = customer.Count();
            var product = await _context.Product.ToListAsync();
            int pro = product.Count();
            var tod = await _context.Order.Where(p => p.OrderDay.Date == DateTime.Now.Date)
                .Where(p => p.OrderDay.Month == DateTime.Now.Month)
                .Where(p => p.OrderDay.Year == DateTime.Now.Year)
                .ToListAsync();
            foreach (var item in tod) {
                today += item.Paid;
            }
            var incom = new List<Income>();

            for (int i = 1 ; i <= 12 ; i++) {
                incom.Add(new Income() { MoneyIn = 0, MoneyOut = 0 });
                var order = await _context.Order.Where(p => p.OrderDay.Month == i).Where(p => p.OrderDay.Year == year).ToListAsync();
                foreach (var item in order) {
                    incom[i - 1].MoneyIn = item.Paid / 100;
                }
                var pur = await _context.Purchase.Where(p => p.PurchaseDay.Month == i).Where(p => p.PurchaseDay.Year == year).ToListAsync();
                foreach (var item in pur) {
                    incom[i - 1].MoneyOut = item.Paid / 100;
                }
            }

            Db2 db2 = new Db2();
            db2.Profit = prof;
            db2.Customer = cus;
            db2.Product = pro;
            db2.Todei = today;
            db2.Year = Convert.ToInt32(year);
            db2.Po = incom;
            return View(db2);
        }

        public async Task<ActionResult> Accecpt(int? id) {
            if (id == null) {
                return NotFound();
            }
            var order = await _context.Order.FindAsync(id);

            if (order.TransactStatus.Equals("shipping"))
                order.TransactStatus = "done";
            if (order.TransactStatus.Equals("accept"))
                order.TransactStatus = "shipping";
            if (order.TransactStatus.Equals("pay by cash") || order.TransactStatus.Equals("paid"))
                order.TransactStatus = "accept";

            _context.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Dashboard1));
        }
    }
}