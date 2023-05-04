using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2.Areas.Order;
using WEB2.Data;
using WEB2.Models;

namespace WEB2.Areas.Admin.Controllers {

    [Area("Admin")]
    [Authorize("Staff")]
    public class InventoryManagersController : Controller {
        private readonly AppDbContext _context;

        public InventoryManagersController(AppDbContext context) {
            _context = context;
        }

        // GET: InventoryManagersController
        public async Task<ActionResult> Index() {
            var inven = await _context.Inventory.ToListAsync();
            return View(inven);
        }

        // GET: InventoryManagersController/Details/5
        public async Task<ActionResult> InvenDetails(int id) {
            var inven = await _context.Invent_Product
                .Include(p => p.Inventory)
                .Include(p => p.Product)
                .Where(p => p.InventoryId == id).ToListAsync();

            return View(inven);
        }

        // GET: InventoryManagersController/Create

        public async Task<ActionResult> TransProduct() {
            var inven = await _context.Invent_Product.Include(p => p.Inventory)
                .Include(p => p.Product)
                .ToListAsync();
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductDetail");
            ViewData["InventoryId"] = new SelectList(_context.Inventory, "InventoryId", "Name");
            return View();
        }

        public async Task<ActionResult> TransProductId(int invenid, int proid) {
            var inven = await _context.Invent_Product.Include(p => p.Inventory)
                .Include(p => p.Product)
                .ToListAsync();
            ViewData["ProductDemo"] = new SelectList(_context.Product.Where(p => p.ProductId == proid), "ProductId", "ProductDetail");
            ViewData["InvenDemo"] = new SelectList(_context.Inventory.Where(p => p.InventoryId == invenid), "InventoryId", "Name");
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductDetail");
            ViewData["InventoryId"] = new SelectList(_context.Inventory, "InventoryId", "Name");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Trans([FromBody] ProductTrans product) {
            for (int i = 0 ; i < product.ProductId.Count ; ++i) {
                var inven1 = await _context.Invent_Product
                    .Where(p => p.InventoryId == product.FirstInvent)
                    .Where(p => p.ProductId == product.ProductId[i]).FirstAsync();
                var inven2 = await _context.Invent_Product
                   .Where(p => p.InventoryId == product.SecondInvent)
                   .Where(p => p.ProductId == product.ProductId[i]).FirstAsync();
                //kiểm tra số lượng sau khi chuyển có hợp lệ hay không
                if (inven1.ProductAvailable - product.Quantity[i] >= 0) {
                    inven1.ProductAvailable -= product.Quantity[i];
                    inven2.ProductAvailable += product.Quantity[i];
                    _context.Update(inven1);
                    await _context.SaveChangesAsync();
                    _context.Update(inven2);
                    await _context.SaveChangesAsync();
                } else {
                }
            }
            return Json(new {
                newUrl = Url.Action("Index", "InventoryManagers")
            });
        }

        // POST: InventoryManagersController/Create
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Inventory collection) {
            _context.Add(collection);
            await _context.SaveChangesAsync();
            //add sp here
            return Json(new {
                newUrl = Url.Action("Index", "InventoryManagers")
            });
        }

        public async Task<ActionResult> Product() {
            var P = new List<PurchaseOrder>();
            var reorder = new List<OrderDetail>();
            var repur = new List<PurchaseDetail>();
            var order = await _context.OrderDetail
                .Include(p => p.Product)
                .Include(p => p.Order)
                .Include(p => p.Order.Customer)
                .Include(p => p.Order.Customer.AppUser)
                .Include(p => p.Order.Payment)
                .Include(p => p.Order.Shipment)
                .Where(p => p.Order.TransactStatus == "accept")
                .ToListAsync();
            if (order.Count > 0) {
                OrderDetail od = new OrderDetail();

                bool check = false;

                for (int i = 0 ; i < order.Count - 1 ; i++) {
                    if (check == false) {
                        od = order[i];
                        od.IDSKU = od.Quantity.ToString();
                    }

                    if (order[i].OrderId == order[i + 1].OrderId) {
                        od.Product.ProductName += "\n" + order[i + 1].Product.ProductName;
                        od.IDSKU += "\n" + order[i + 1].Quantity.ToString();
                        check = true;
                    } else {
                        reorder.Add(od);
                        check = false;
                    }
                }
                if (check == false) {
                    od = order[order.Count - 1];
                    od.IDSKU = od.Quantity.ToString();
                }
                reorder.Add(od);
            }
            var pur = await _context.PurchaseDetail
                .Include(p => p.Purchase)
                .Include(p => p.Product)
                .Include(p => p.Purchase.Supplier)
                .Include(p => p.Purchase.Staff)
                .Where(p => p.Purchase.TransactStatus == "sent")
                .ToListAsync();
            if (pur.Count > 0) {
                PurchaseDetail purde = new PurchaseDetail();

                bool flag = false;

                for (int i = 0 ; i < pur.Count - 1 ; i++) {
                    if (flag == false) {
                        purde = pur[i];
                        purde.IDSKU = purde.Quantity.ToString();
                    }

                    if (pur[i].PurchaseId == pur[i + 1].PurchaseId) {
                        purde.Product.ProductName += "\n" + pur[i + 1].Product.ProductName;
                        purde.IDSKU += "\n" + pur[i + 1].Quantity.ToString();
                        flag = true;
                    } else {
                        repur.Add(purde);
                        flag = false;
                    }
                }
                if (flag == false) {
                    purde = pur[pur.Count - 1];
                    purde.IDSKU = purde.Quantity.ToString();
                }
                repur.Add(purde);
            }
            foreach (var item in reorder) {
                var p = new PurchaseOrder {
                    OrderId = item.OrderId,
                    ProductId = item.ProductId,
                    ProductName = item.Product.ProductName,
                    Paid = item.Order.Paid,
                    Quantity = item.IDSKU,
                    DayTransaction = item.Order.OrderDay,
                    Type = item.Order.TransactStatus,
                    Fullname = item.Order.Customer.AppUser.FullName,
                    InvenId = item.Order.InventoryId
                };
                P.Add(p);
            }

            foreach (var item in repur) {
                var p = new PurchaseOrder() {
                    OrderId = item.PurchaseId,
                    ProductId = item.ProductId,
                    ProductName = item.Product.ProductName,
                    Quantity = item.IDSKU,
                    Paid = item.Purchase.Paid,
                    DayTransaction = item.Purchase.PurchaseDay,
                    Type = item.Purchase.TransactStatus,
                    Fullname = item.Purchase.Supplier.CompanyName,
                    InvenId = item.Purchase.InventoryId
                };

                P.Add(p);
            }

            return View(P);
        }

        public async Task<ActionResult> Accept_O(int id) {
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
            return RedirectToAction(nameof(Product));
        }

        public async Task<ActionResult> Details_O(int id) {
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

        public async Task<ActionResult> BillDel_O(int id) {
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
            return RedirectToAction(nameof(Product));
        }

        public async Task<IActionResult> Accept_P(int id) {
            var pur = await _context.Purchase.FindAsync(id);
            if (pur.TransactStatus.Equals("receive") || pur.TransactStatus.Equals("left")) {
                var purdetail = await _context.PurchaseDetail
                    .Include(p => p.Product)
                    .Include(p => p.Purchase)
                    .Include(p => p.Purchase.Staff)
                    .Include(p => p.Purchase.Supplier)
                    .Include(p => p.Purchase.Staff.AppUser)
                    .Where(p => p.PurchaseId == id)
                    .Where(p => p.Purchase.TransactStatus == "receive")
                    .ToListAsync();
                return View(purdetail);
            }

            if (pur.TransactStatus.Equals("sent")) {
                pur.TransactStatus = "receive";
                var purdetail = await _context.PurchaseDetail
                    .Include(p => p.Product)
                    .Include(p => p.Purchase)
                    .Where(p => p.PurchaseId == id)
                    .Where(p => p.Purchase.TransactStatus == "sent")
                    .ToListAsync();
                foreach (var item in purdetail) {
                    var pro = await _context.Product.Where(p => p.ProductId == item.ProductId).FirstOrDefaultAsync();
                    pro.ReorderLevel += item.Quantity;
                    _context.Update(pro);
                    await _context.SaveChangesAsync();
                }
            }
            if (pur.TransactStatus.Equals("saved"))
                pur.TransactStatus = "sent";

            _context.Update(pur);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Product));
        }

        public async Task<IActionResult> Details_P(int? id) {
            if (id == null) {
                return NotFound();
            }

            var pur = await _context.PurchaseDetail
                .Include(p => p.Purchase)
                .Include(p => p.Purchase.Staff)
                .Include(p => p.Purchase.Supplier)
                .Include(p => p.Product)
                .Include(p => p.Purchase.Staff.AppUser)
                .Where(p => p.PurchaseId == id)
                .ToListAsync();

            return View(pur);
        }

        public async Task<IActionResult> Delete_P(int? id) {
            if (id == null) {
                return NotFound();
            }

            var purchaseDetail = await _context.Purchase.FindAsync(id);

            if (purchaseDetail == null) {
                return NotFound();
            }
            purchaseDetail.TransactStatus = "cancel";

            _context.Update(purchaseDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Product));
        }

        // GET: InventoryManagersController/Edit/5
        public ActionResult Edit(int id) {
            return View();
        }

        // POST: InventoryManagersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection) {
            try {
                return RedirectToAction(nameof(Index));
            } catch {
                return View();
            }
        }

        // GET: InventoryManagersController/Delete/5
        public ActionResult Delete(int id) {
            return View();
        }

        // POST: InventoryManagersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection) {
            try {
                return RedirectToAction(nameof(Index));
            } catch {
                return View();
            }
        }
    }
}