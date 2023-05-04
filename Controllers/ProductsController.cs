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

namespace WEB2.Controllers {

    public class ProductsController : Controller {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public ProductsController(AppDbContext context, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager) {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Laptop
        public async Task<IActionResult> Laptop() {
            var products = await _context.Product.Include(p => p.Battery).Include(p => p.Camera).Include(p => p.Category).Include(p => p.Connection).Include(p => p.Graphic).Include(p => p.OS).Include(p => p.Processor).Include(p => p.Ram).Include(p => p.Rom).Include(p => p.Screen).Include(p => p.Sound).Include(p => p.Structure)
                .Where(p => p.Category.ParentCategoryId == 2).ToListAsync();
            var pro = new List<Product>();
            var protemp = new Product();
            bool check = false;
            if (products.Count <= 1) {
                return View(products);
            }
            for (int i = 0 ; i < products.Count - 1 ; i++) {
                if (check == false) {
                    protemp = products[i];
                    protemp.ProductName = products[i].ProductName;
                }
                if (products[i].ProductName == products[i + 1].ProductName) {
                    check = true;
                    continue;
                } else {
                    pro.Add(protemp);
                    check = false;
                }
            }
            if (check == false) {
                pro.Add(products[products.Count - 1]);
            }
            pro.Add(protemp);
            return View(pro);
        }

        // GET: Phone
        public async Task<IActionResult> Phone() {
            var products = await _context.Product.Include(p => p.Battery).Include(p => p.Camera).Include(p => p.Category).Include(p => p.Connection).Include(p => p.Graphic).Include(p => p.OS).Include(p => p.Processor).Include(p => p.Ram).Include(p => p.Rom).Include(p => p.Screen).Include(p => p.Sound).Include(p => p.Structure)
                .Where(p => p.Category.ParentCategoryId == 1).ToListAsync();
            var pro = new List<Product>();
            var protemp = new Product();
            bool check = false;
            if (products.Count <= 1) {
                return View(products);
            }
            for (int i = 0 ; i < products.Count - 1 ; i++) {
                if (check == false) {
                    protemp = products[i];
                    protemp.ProductName = products[i].ProductName;
                }
                if (products[i].ProductName == products[i + 1].ProductName) {
                    check = true;
                    continue;
                } else {
                    pro.Add(protemp);
                    check = false;
                }
            }
            if (check == false) {
                pro.Add(products[products.Count - 1]);
            }
            pro.Add(protemp);
            return View(pro);
        }

        // GET: Tablet
        public async Task<IActionResult> Tablet() {
            var products = await _context.Product.Include(p => p.Battery).Include(p => p.Camera).Include(p => p.Category).Include(p => p.Connection).Include(p => p.Graphic).Include(p => p.OS).Include(p => p.Processor).Include(p => p.Ram).Include(p => p.Rom).Include(p => p.Screen).Include(p => p.Sound).Include(p => p.Structure)
                .Where(p => p.Category.ParentCategoryId == 3).ToListAsync();
            var pro = new List<Product>();
            var protemp = new Product();
            bool check = false;
            if (products.Count <= 1) {
                return View(products);
            }
            for (int i = 0 ; i < products.Count - 1 ; i++) {
                if (check == false) {
                    protemp = products[i];
                    protemp.ProductName = products[i].ProductName;
                }
                if (products[i].ProductName == products[i + 1].ProductName) {
                    check = true;
                    continue;
                } else {
                    pro.Add(protemp);
                    check = false;
                }
            }
            if (check == false) {
                pro.Add(products[products.Count - 1]);
            }
            pro.Add(protemp);
            return View(pro);
        }

        // GET: Watch

        // GET: Sound
        public async Task<IActionResult> Sound() {
            var products = await _context.Product.Include(p => p.Battery).Include(p => p.Camera).Include(p => p.Category).Include(p => p.Connection).Include(p => p.Graphic).Include(p => p.OS).Include(p => p.Processor).Include(p => p.Ram).Include(p => p.Rom).Include(p => p.Screen).Include(p => p.Sound).Include(p => p.Structure)
                .Where(p => p.Category.ParentCategoryId == 5).ToListAsync();
            var pro = new List<Product>();
            var protemp = new Product();
            bool check = false;
            if (products.Count <= 1) {
                return View(products);
            }
            for (int i = 0 ; i < products.Count - 1 ; i++) {
                if (check == false) {
                    protemp = products[i];
                    protemp.ProductName = products[i].ProductName;
                }
                if (products[i].ProductName == products[i + 1].ProductName) {
                    check = true;
                    continue;
                } else {
                    pro.Add(protemp);
                    check = false;
                }
            }
            if (check == false) {
                pro.Add(products[products.Count - 1]);
            }
            pro.Add(protemp);
            return View(pro);
        }

        // GET: Accessories
        public async Task<IActionResult> Accessories() {
            var products = await _context.Product.Include(p => p.Battery).Include(p => p.Camera).Include(p => p.Category).Include(p => p.Connection).Include(p => p.Graphic).Include(p => p.OS).Include(p => p.Processor).Include(p => p.Ram).Include(p => p.Rom).Include(p => p.Screen).Include(p => p.Sound).Include(p => p.Structure)
                .Where(p => p.Category.ParentCategoryId == 6).ToListAsync();
            var pro = new List<Product>();
            var protemp = new Product();
            bool check = false;
            if (products.Count <= 1) {
                return View(products);
            }
            for (int i = 0 ; i < products.Count - 1 ; i++) {
                if (check == false) {
                    protemp = products[i];
                    protemp.ProductName = products[i].ProductName;
                }
                if (products[i].ProductName == products[i + 1].ProductName) {
                    check = true;
                    continue;
                } else {
                    pro.Add(protemp);
                    check = false;
                }
            }
            if (check == false) {
                pro.Add(products[products.Count - 1]);
            }
            pro.Add(protemp);
            return View(pro);
        }

        public async Task<IActionResult> Category(int? id) {
            if (id == null) {
                return NotFound();
            }
            var products = await _context.Product.Include(p => p.Battery).Include(p => p.Camera).Include(p => p.Category).Include(p => p.Connection).Include(p => p.Graphic).Include(p => p.OS).Include(p => p.Processor).Include(p => p.Ram).Include(p => p.Rom).Include(p => p.Screen).Include(p => p.Sound).Include(p => p.Structure)
                            .Where(p => p.Category.CategoryId == id).ToListAsync();
            var pro = new List<Product>();
            var protemp = new Product();
            bool check = false;
            if (products.Count <= 1) {
                return View(products);
            }
            for (int i = 0 ; i < products.Count - 1 ; i++) {
                if (check == false) {
                    protemp = products[i];
                    protemp.ProductName = products[i].ProductName;
                }
                if (products[i].ProductName == products[i + 1].ProductName) {
                    check = true;
                    continue;
                } else {
                    pro.Add(protemp);
                    check = false;
                }
            }
            if (check == false) {
                pro.Add(products[products.Count - 1]);
            }
            pro.Add(protemp);
            return View(pro);
        }

        public async Task<IActionResult> ExDetails(int? id) {
            if (id == null) {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.Battery)
                .Include(p => p.Camera)
                .Include(p => p.Category)
                .Include(p => p.Connection)
                .Include(p => p.Graphic)
                .Include(p => p.OS)
                .Include(p => p.Processor)
                .Include(p => p.Ram)
                .Include(p => p.Rom)
                .Include(p => p.Screen)
                .Include(p => p.Sound)
                .Include(p => p.Structure)
                .Include(p => p.Feedbacks)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null) {
                return NotFound();
            }
            ++product.View;
            _context.Update(product);
            await _context.SaveChangesAsync();
            foreach (var item in product.Feedbacks) {
                if (item.IsShow == false)
                    item.Rate = -1;
            }
            var pro = await _context.Product
            .Where(p => p.ProductName == product.ProductName)
            .ToListAsync();
            var col = await _context.Product.Where(p => p.ProductName == product.ProductName)
                .Where(p => p.Version == product.Version).ToListAsync();
            string color = "";
            foreach (var item in col) {
                color += "-" + item.Color;
            }

            string version = "";

            foreach (var item in pro) {
                version += "-" + item.Version;
            }

            product.Category.Active = version;
            product.Category.Picture = color;

            return View(product);
        }

        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }
            var product = await _context.ProductContent.Include(p => p.Product).Include(p => p.Content)
                .Include(p => p.Product.Category)
                .Include(p => p.Product.Battery)
                .Include(p => p.Product.Camera)
                .Include(p => p.Product.Category)
                .Include(p => p.Product.Connection)
                .Include(p => p.Product.Graphic)
                .Include(p => p.Product.OS)
                .Include(p => p.Product.Processor)
                .Include(p => p.Product.Ram)
                .Include(p => p.Product.Rom)
                .Include(p => p.Product.Screen)
                .Include(p => p.Product.Sound)
                .Include(p => p.Product.Structure)
                .Include(p => p.Product.Feedbacks)
                .Include(p => p.Product.Images)
                .Include(p => p.Product.ProductDiscounts)
                .FirstOrDefaultAsync(m => m.Product.ProductId == id);

            if (product == null) {
                return RedirectToAction("ExDetails", new { id = id });
            }

            ++product.Product.View;
            _context.Update(product);
            await _context.SaveChangesAsync();
            foreach (var item in product.Product.Feedbacks) {
                if (item.IsShow == false)
                    item.Rate = -1;
            }

            var pro = await _context.Product
                .Where(p => p.ProductName == product.Product.ProductName)
                .ToListAsync();
            var col = await _context.Product.Where(p => p.ProductName == product.Product.ProductName)
                .Where(p => p.Version == product.Product.Version).ToListAsync();
            string color = "";
            foreach (var item in col) {
                color += "-" + item.Color;
            }

            string version = "";

            foreach (var item in pro) {
                version += "-" + item.Version;
            }

            product.Product.Category.Active = version;
            product.Product.Category.Picture = color;

            return View(product);
        }

        public async Task<IActionResult> DetailVersion(string productname, string version) {
            var product = await _context.Product.Where(p => p.ProductName == productname)
                .Where(p => p.Version == version).FirstOrDefaultAsync();
            ++product.View;
            _context.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = product.ProductId });
        }

        public async Task<IActionResult> DetailColor(string productname, string version, string color) {
            var product = await _context.Product.Where(p => p.ProductName == productname)
          .Where(p => p.Version == version).Where(p => p.Color == color).FirstOrDefaultAsync();
            ++product.View;
            _context.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = product.ProductId });
        }

        [HttpPost]
        public async Task<IActionResult> Rate([FromBody] Feedback feedback) {
            var user = await _userManager.GetUserAsync(User);

            var userid = await _userManager.GetUserIdAsync(user);

            var customer = await _context.Users.FindAsync(userid);

            if (ModelState.IsValid) {
                feedback.FeedbackDay = DateTime.Now;
                feedback.IsShow = false;
                feedback.userid = customer.Id;

                _context.Add(feedback);
                await _context.SaveChangesAsync();
                return Json(new {
                    newUrl = Url.Action("Details", "Products", new { id = feedback.ProductId })
                });
            }

            return Json(new {
                newUrl = Url.Action("Details", "Products", new { id = feedback.ProductId })
            });
        }

        [HttpPost]
        public async Task<IActionResult> Repcmt(string tex, int repcom, int pro) {
            var user = await _userManager.GetUserAsync(User);

            var userid = await _userManager.GetUserIdAsync(user);

            var customer = await _context.Users.FindAsync(userid);

            if (ModelState.IsValid) {
                Feedback feedback = new Feedback {
                    FeedbackDay = DateTime.Now,
                    IsShow = false,
                    userid = customer.Id,
                    Comment = tex,
                    repid = repcom,
                    ProductId = pro,
                    Rank = customer.FullName
                };

                _context.Add(feedback);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), pro);
        }
    }
}