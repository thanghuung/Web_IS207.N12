using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB2.Areas.Order;
using WEB2.Data;
using WEB2.Models;

namespace WEB2.Areas.Admin.Controllers {

    [Area("Admin")]
    public class PurchaseDetailsController : Controller {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public PurchaseDetailsController(AppDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // GET: Admin/PurchaseDetails
        public async Task<IActionResult> Transaction() {
            var pur = await _context.PurchaseDetail.Include(p => p.Product).Include(p => p.Purchase).ToListAsync();
            if (pur.Count == 0) {
                return View(pur);
            }
            PurchaseDetail od = new PurchaseDetail();
            var reorder = new List<PurchaseDetail>();
            bool check = false;

            for (int i = 0 ; i < pur.Count - 1 ; i++) {
                if (check == false) {
                    od = pur[i];
                    od.IDSKU = od.Quantity.ToString();
                }

                if (pur[i].PurchaseId == pur[i + 1].PurchaseId) {
                    od.Product.ProductName += "\n" + pur[i + 1].Product.ProductName;
                    od.IDSKU += "\n" + pur[i + 1].Quantity.ToString();
                    check = true;
                } else {
                    reorder.Add(od);
                    check = false;
                }
            }
            if (check == false) {
                od = pur[pur.Count - 1];
                od.IDSKU = od.Quantity.ToString();
            }
            reorder.Add(od);

            return View(reorder);
        }

        public async Task<IActionResult> Receipt() {
            var pur = await _context.PurchaseDetail.Include(p => p.Product).Include(p => p.Purchase)
                .Where(p => p.Purchase.TransactStatus != "saved")
                .Where(p => p.Purchase.TransactStatus != "sent")
                .Where(p => p.Purchase.TransactStatus != "left").ToListAsync();
            if (pur.Count == 0) {
                return View(pur);
            }
            PurchaseDetail od = new PurchaseDetail();
            var reorder = new List<PurchaseDetail>();
            bool check = false;

            for (int i = 0 ; i < pur.Count - 1 ; i++) {
                if (check == false) {
                    od = pur[i];
                    od.IDSKU = od.Quantity.ToString();
                }

                if (pur[i].PurchaseId == pur[i + 1].PurchaseId) {
                    od.Product.ProductName += "\n" + pur[i + 1].Product.ProductName;
                    od.IDSKU += "\n" + pur[i + 1].Quantity.ToString();
                    check = true;
                } else {
                    reorder.Add(od);
                    check = false;
                }
            }
            if (check == false) {
                od = pur[pur.Count - 1];
                od.IDSKU = od.Quantity.ToString();
            }
            reorder.Add(od);

            return View(reorder);
        }

        public async Task<IActionResult> Requests() {
            //“saved”, “sent”, “receive”,”left” ,“done”, “cancel”
            var pur = await _context.PurchaseDetail.Include(p => p.Purchase)
                .Include(p => p.Product)
                .Include(p => p.Purchase.Staff)
                .Where(p => p.Purchase.TransactStatus != "done")
                .Where(p => p.Purchase.TransactStatus != "cancel")
                .Where(p => p.Purchase.TransactStatus != "receive")
                .Where(p => p.Purchase.TransactStatus != "left")
                .ToListAsync();

            if (pur.Count == 0) {
                return View(pur);
            }
            PurchaseDetail od = new PurchaseDetail();
            var reorder = new List<PurchaseDetail>();
            bool check = false;

            for (int i = 0 ; i < pur.Count - 1 ; i++) {
                if (check == false) {
                    od = pur[i];
                    od.IDSKU = od.Quantity.ToString();
                }

                if (pur[i].PurchaseId == pur[i + 1].PurchaseId) {
                    od.Product.ProductName += "\n" + pur[i + 1].Product.ProductName;
                    od.IDSKU += "\n" + pur[i + 1].Quantity.ToString();
                    check = true;
                } else {
                    reorder.Add(od);
                    check = false;
                }
            }
            if (check == false) {
                od = pur[pur.Count - 1];
                od.IDSKU = od.Quantity.ToString();
            }
            reorder.Add(od);

            return View(reorder);
        }

        public async Task<IActionResult> Done() {
            var pur = await _context.PurchaseDetail.Include(p => p.Product).Include(p => p.Purchase)
                .Where(p => p.Purchase.TransactStatus == "done").ToListAsync();
            if (pur.Count == 0) {
                return View(pur);
            }
            PurchaseDetail od = new PurchaseDetail();
            var reorder = new List<PurchaseDetail>();
            bool check = false;

            for (int i = 0 ; i < pur.Count - 1 ; i++) {
                if (check == false) {
                    od = pur[i];
                    od.IDSKU = od.Quantity.ToString();
                }

                if (pur[i].PurchaseId == pur[i + 1].PurchaseId) {
                    od.Product.ProductName += "\n" + pur[i + 1].Product.ProductName;
                    od.IDSKU += "\n" + pur[i + 1].Quantity.ToString();
                    check = true;
                } else {
                    reorder.Add(od);
                    check = false;
                }
            }
            if (check == false) {
                od = pur[pur.Count - 1];
                od.IDSKU = od.Quantity.ToString();
            }
            reorder.Add(od);

            return View(reorder);
        }

        public async Task<IActionResult> Cancel() {
            var pur = await _context.PurchaseDetail.Include(p => p.Product).Include(p => p.Purchase)
                .Where(p => p.Purchase.TransactStatus == "cancel").ToListAsync();
            if (pur.Count == 0) {
                return View(pur);
            }
            PurchaseDetail od = new PurchaseDetail();
            var reorder = new List<PurchaseDetail>();
            bool check = false;

            for (int i = 0 ; i < pur.Count - 1 ; i++) {
                if (check == false) {
                    od = pur[i];
                    od.IDSKU = od.Quantity.ToString();
                }

                if (pur[i].PurchaseId == pur[i + 1].PurchaseId) {
                    od.Product.ProductName += "\n" + pur[i + 1].Product.ProductName;
                    od.IDSKU += "\n" + pur[i + 1].Quantity.ToString();
                    check = true;
                } else {
                    reorder.Add(od);
                    check = false;
                }
            }
            if (check == false) {
                od = pur[pur.Count - 1];
                od.IDSKU = od.Quantity.ToString();
            }
            reorder.Add(od);

            return View(reorder);
        }

        // GET: Admin/PurchaseDetails/Create
        public IActionResult Create() {
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductDetail");
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "SupplierId", "CompanyName");
            ViewData["InventoryId"] = new SelectList(_context.Inventory, "InventoryId", "Name");
            return View();
        }

        public IActionResult CreateID(int id) {
            ViewData["ProductDemo"] = new SelectList(_context.Product.Where(p => p.ProductId == id), "ProductId", "ProductDetail");
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductDetail");
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "SupplierId", "CompanyName");
            ViewData["InventoryId"] = new SelectList(_context.Inventory, "InventoryId", "Name");

            return View();
        }

        // POST: Admin/PurchaseDetails/Create To protect from overposting attacks, enable the
        // specific properties you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        public async Task<IActionResult> CreateBill([FromBody] PurchaseItem purchaseDetail) {
            var user = await _userManager.GetUserAsync(User);
            var userid = await _userManager.GetUserIdAsync(user);
            if (ModelState.IsValid) {
                var staff = await _context.Staff.Where(p => p.UserId == userid).FirstOrDefaultAsync();
                //tao hoa don mua hang
                var purchase = new Purchase {
                    SupplierId = purchaseDetail.SupplierId,
                    StaffId = staff.StaffId,
                    PurchaseDay = DateTime.Now,
                    TransactStatus = "new",
                    Paid = purchaseDetail.Paid,
                    InventoryId = purchaseDetail.InvenId
                };
                _context.Add(purchase);
                await _context.SaveChangesAsync();
                //tap chi tiet hoa don mua hang
                var pur = await _context.Purchase.FirstOrDefaultAsync(p => p.TransactStatus == "new");
                for (int i = 0 ; i < purchaseDetail.Productid.Count ; i++) {
                    var purdetail = new PurchaseDetail {
                        PurchaseId = pur.PurchaseId,
                        ProductId = purchaseDetail.Productid[i],
                        Quantity = purchaseDetail.Quantity[i],
                        Status = "saved",
                        Price = purchaseDetail.Price[i],
                        Total = purchaseDetail.Price[i] * purchaseDetail.Quantity[i]
                    };
                    _context.Add(purdetail);
                    await _context.SaveChangesAsync();
                }
                //cap nhat lai status
                pur.TransactStatus = "saved";
                _context.Update(pur);
                await _context.SaveChangesAsync();

                // return RedirectToAction(nameof(Requests));
                return Json(new {
                    newUrl = Url.Action("Requests", "PurchaseDetails")
                });
            }

            return View(purchaseDetail);
        }

        /// <summary>
        /// transaction
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        public async Task<IActionResult> Accept_T(int id) {
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
                pur.TransactStatus = "done";
                pur.PaymentDate = DateTime.Now;

                foreach (var item in purdetail) {
                    item.Status = "received";
                    //cap nhat so luong
                    var inv = await _context.Invent_Product.Where(p => p.InventoryId == pur.InventoryId).Where(p => p.ProductId == item.ProductId).FirstOrDefaultAsync();
                    inv.ProductAvailable += item.Quantity;
                    _context.Update(inv);
                    await _context.SaveChangesAsync();
                    //update giá gốc của sản phẩm
                    //var pro = await _context.Product.FindAsync(item.ProductId);
                    //pro.RawPrice = item.Price;
                    //_context.Update(pro);
                    //await _context.SaveChangesAsync();

                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
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
            return RedirectToAction(nameof(Transaction));
        }

        public async Task<IActionResult> Details_T(int? id) {
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

        // GET: Admin/PurchaseDetails/Delete/5
        public async Task<IActionResult> Delete_T(int? id) {
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
            return RedirectToAction(nameof(Transaction));
        }

        /// <summary>
        /// request
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        public async Task<IActionResult> Accept_RQ(int id) {
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
            return RedirectToAction(nameof(Requests));
        }

        public async Task<IActionResult> Details_RQ(int? id) {
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

        // GET: Admin/PurchaseDetails/Delete/5
        public async Task<IActionResult> Delete_RQ(int? id) {
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
            return RedirectToAction(nameof(Requests));
        }

        /// <summary>
        /// receipt
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        public async Task<IActionResult> Accept_R(int id) {
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
            return RedirectToAction(nameof(Receipt));
        }

        public async Task<IActionResult> Details_R(int? id) {
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

        // GET: Admin/PurchaseDetails/Delete/5
        public async Task<IActionResult> Delete_R(int? id) {
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
            return RedirectToAction(nameof(Receipt));
        }

        private bool PurchaseDetailExists(int id) {
            return _context.PurchaseDetail.Any(e => e.ProductId == id);
        }
    }
}