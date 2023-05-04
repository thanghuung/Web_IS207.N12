using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB2.Data;
using WEB2.Models;

namespace WEB2.Areas.Admin.Controllers {

    [Area("Admin")]
    [Authorize("Admin")]
    public class ShipmentsController : Controller {
        private readonly AppDbContext _context;

        public ShipmentsController( AppDbContext context ) {
            _context = context;
        }

        // GET: Admin/Shipments
        public async Task<IActionResult> Index() {
            return View(await _context.Shipment.ToListAsync());
        }

        // GET: Admin/Shipments/Details/5
        public async Task<IActionResult> Details( int? id ) {
            if (id == null) {
                return NotFound();
            }

            var shipment = await _context.Shipment
                .FirstOrDefaultAsync(m => m.ShipperId == id);
            if (shipment == null) {
                return NotFound();
            }

            return View(shipment);
        }

        // GET: Admin/Shipments/Create
        public IActionResult Create() {
            return View();
        }

        // POST: Admin/Shipments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( [Bind("ShipperId,CompanyName,Phone,ShipDate,Received")] Shipment shipment ) {
            if (ModelState.IsValid) {
                _context.Add(shipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shipment);
        }

        // GET: Admin/Shipments/Edit/5
        public async Task<IActionResult> Edit( int? id ) {
            if (id == null) {
                return NotFound();
            }

            var shipment = await _context.Shipment.FindAsync(id);
            if (shipment == null) {
                return NotFound();
            }
            return View(shipment);
        }

        // POST: Admin/Shipments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( int id, [Bind("ShipperId,CompanyName,Phone,ShipDate,Received")] Shipment shipment ) {
            if (id != shipment.ShipperId) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(shipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!ShipmentExists(shipment.ShipperId)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(shipment);
        }

        // GET: Admin/Shipments/Delete/5
        public async Task<IActionResult> Delete( int? id ) {
            if (id == null) {
                return NotFound();
            }

            var shipment = await _context.Shipment
                .FirstOrDefaultAsync(m => m.ShipperId == id);
            if (shipment == null) {
                return NotFound();
            }

            return View(shipment);
        }

        // POST: Admin/Shipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed( int id ) {
            var shipment = await _context.Shipment.FindAsync(id);
            _context.Shipment.Remove(shipment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShipmentExists( int id ) {
            return _context.Shipment.Any(e => e.ShipperId == id);
        }
    }
}