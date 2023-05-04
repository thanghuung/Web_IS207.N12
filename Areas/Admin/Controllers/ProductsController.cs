using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB2.Data;
using WEB2.Models;

namespace WEB2.Areas.Admin.Controllers {

    [Area("Admin")]
    public class ProductsController : Controller {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context) {
            _context = context;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index() {
            var appDbContext = _context.Product
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
                .Where(p => p.IsDelete == false);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Recycle() {
            var appDbContext = _context.Product
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
                .Where(p => p.IsDelete == true);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var product = await _context.Product
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
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null) {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create() {
            ViewData["BatteryID"] = new SelectList(_context.Battery, "BatteryId", "BatteryId");
            ViewData["CamID"] = new SelectList(_context.Set<Camera>(), "CamId", "CamId");
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId");
            ViewData["ConnID"] = new SelectList(_context.Set<Connection>(), "ConnId", "ConnId");
            ViewData["GraphicID"] = new SelectList(_context.Set<Graphic>(), "GraphicId", "GraphicId");
            ViewData["InventoryId"] = new SelectList(_context.Inventory, "InventoryId", "InventoryId");
            ViewData["OSID"] = new SelectList(_context.Set<OS>(), "OsId", "OsId");
            ViewData["CPUID"] = new SelectList(_context.Set<Processor>(), "CpuId", "CpuId");
            ViewData["RamID"] = new SelectList(_context.Set<Ram>(), "RamId", "RamId");
            ViewData["RomID"] = new SelectList(_context.Set<Rom>(), "RomId", "RomId");
            ViewData["ScreenID"] = new SelectList(_context.Set<Screen>(), "ScreenId", "ScreenId");
            ViewData["SoundID"] = new SelectList(_context.Set<Sound>(), "SoundId", "SoundId");
            ViewData["StructID"] = new SelectList(_context.Set<Structure>(), "StructId", "StructId");
            return View();
        }

        // POST: Admin/Products/Create To protect from overposting attacks, enable the specific
        // properties you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,InventoryId,CategoryId,ProductName,UnitPrice,View,Picture,RawPrice,VendorProductId,ProductDetail,MSRP,AvailableVersion,Version,AvailableColor,Color,UnitInStock,ProductAvailable,UnitInOrder,ReorderLevel,CurrentOrder,Note,ConnID,ScreenID,StructID,SoundID,GraphicID,BatteryID,RamID,OSID,CamID,CPUID,RomID")] Product product) {
            if (ModelState.IsValid) {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BatteryID"] = new SelectList(_context.Battery, "BatteryId", "BatteryId", product.BatteryID);
            ViewData["CamID"] = new SelectList(_context.Set<Camera>(), "CamId", "CamId", product.CamID);
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["ConnID"] = new SelectList(_context.Set<Connection>(), "ConnId", "ConnId", product.ConnID);
            ViewData["GraphicID"] = new SelectList(_context.Set<Graphic>(), "GraphicId", "GraphicId", product.GraphicID);

            ViewData["OSID"] = new SelectList(_context.Set<OS>(), "OsId", "OsId", product.OSID);
            ViewData["CPUID"] = new SelectList(_context.Set<Processor>(), "CpuId", "CpuId", product.CPUID);
            ViewData["RamID"] = new SelectList(_context.Set<Ram>(), "RamId", "RamId", product.RamID);
            ViewData["RomID"] = new SelectList(_context.Set<Rom>(), "RomId", "RomId", product.RomID);
            ViewData["ScreenID"] = new SelectList(_context.Set<Screen>(), "ScreenId", "ScreenId", product.ScreenID);
            ViewData["SoundID"] = new SelectList(_context.Set<Sound>(), "SoundId", "SoundId", product.SoundID);
            ViewData["StructID"] = new SelectList(_context.Set<Structure>(), "StructId", "StructId", product.StructID);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null) {
                return NotFound();
            }
            ViewData["BatteryID"] = new SelectList(_context.Battery, "BatteryId", "BatteryId", product.BatteryID);
            ViewData["CamID"] = new SelectList(_context.Set<Camera>(), "CamId", "CamId", product.CamID);
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["ConnID"] = new SelectList(_context.Set<Connection>(), "ConnId", "ConnId", product.ConnID);
            ViewData["GraphicID"] = new SelectList(_context.Set<Graphic>(), "GraphicId", "GraphicId", product.GraphicID);

            ViewData["OSID"] = new SelectList(_context.Set<OS>(), "OsId", "OsId", product.OSID);
            ViewData["CPUID"] = new SelectList(_context.Set<Processor>(), "CpuId", "CpuId", product.CPUID);
            ViewData["RamID"] = new SelectList(_context.Set<Ram>(), "RamId", "RamId", product.RamID);
            ViewData["RomID"] = new SelectList(_context.Set<Rom>(), "RomId", "RomId", product.RomID);
            ViewData["ScreenID"] = new SelectList(_context.Set<Screen>(), "ScreenId", "ScreenId", product.ScreenID);
            ViewData["SoundID"] = new SelectList(_context.Set<Sound>(), "SoundId", "SoundId", product.SoundID);
            ViewData["StructID"] = new SelectList(_context.Set<Structure>(), "StructId", "StructId", product.StructID);
            return View(product);
        }

        // POST: Admin/Products/Edit/5 To protect from overposting attacks, enable the specific
        // properties you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,InventoryId,CategoryId,ProductName,UnitPrice,View,Picture,RawPrice,VendorProductId,ProductDetail,MSRP,AvailableVersion,Version,AvailableColor,Color,UnitInStock,ProductAvailable,UnitInOrder,ReorderLevel,CurrentOrder,Note,ConnID,ScreenID,StructID,SoundID,GraphicID,BatteryID,RamID,OSID,CamID,CPUID,RomID")] Product product) {
            if (id != product.ProductId) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!ProductExists(product.ProductId)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BatteryID"] = new SelectList(_context.Battery, "BatteryId", "BatteryId", product.BatteryID);
            ViewData["CamID"] = new SelectList(_context.Set<Camera>(), "CamId", "CamId", product.CamID);
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["ConnID"] = new SelectList(_context.Set<Connection>(), "ConnId", "ConnId", product.ConnID);
            ViewData["GraphicID"] = new SelectList(_context.Set<Graphic>(), "GraphicId", "GraphicId", product.GraphicID);

            ViewData["OSID"] = new SelectList(_context.Set<OS>(), "OsId", "OsId", product.OSID);
            ViewData["CPUID"] = new SelectList(_context.Set<Processor>(), "CpuId", "CpuId", product.CPUID);
            ViewData["RamID"] = new SelectList(_context.Set<Ram>(), "RamId", "RamId", product.RamID);
            ViewData["RomID"] = new SelectList(_context.Set<Rom>(), "RomId", "RomId", product.RomID);
            ViewData["ScreenID"] = new SelectList(_context.Set<Screen>(), "ScreenId", "ScreenId", product.ScreenID);
            ViewData["SoundID"] = new SelectList(_context.Set<Sound>(), "SoundId", "SoundId", product.SoundID);
            ViewData["StructID"] = new SelectList(_context.Set<Structure>(), "StructId", "StructId", product.StructID);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var product = await _context.Product
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
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null) {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var product = await _context.Product.FindAsync(id);
            product.IsDelete = true;
            _context.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Recover(int? id) {
            if (id == null) {
                return NotFound();
            }

            var pro = await _context.Product.FindAsync(id);
            pro.IsDelete = false;
            _context.Update(pro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id) {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}