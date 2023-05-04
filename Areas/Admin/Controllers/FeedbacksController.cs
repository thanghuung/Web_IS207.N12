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
    public class FeedbacksController : Controller {
        private readonly AppDbContext _context;

        public FeedbacksController(AppDbContext context) {
            _context = context;
        }

        // GET: Admin/Feedbacks
        public async Task<IActionResult> Index() {
            var appDbContext = _context.Feedback.Include(f => f.Product);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Accept(int? id) {
            var feedback = await _context.Feedback.FirstOrDefaultAsync(f => f.FeedbackId == id);
            feedback.IsShow = true;
            _context.Update(feedback);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Feedbacks/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var feedback = await _context.Feedback
                .Include(f => f.Product)
                .FirstOrDefaultAsync(m => m.FeedbackId == id);
            if (feedback == null) {
                return NotFound();
            }

            return View(feedback);
        }

        // GET: Admin/Feedbacks/Create
        public IActionResult Create() {
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId");
            return View();
        }

        // POST: Admin/Feedbacks/Create To protect from overposting attacks, enable the specific
        // properties you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FeedbackId,ProductId,Rank,Comment,Rate,CustomerId,isShow,FeedbackDay")] Feedback feedback) {
            if (ModelState.IsValid) {
                _context.Add(feedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", feedback.ProductId);
            return View(feedback);
        }

        // GET: Admin/Feedbacks/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var feedback = await _context.Feedback.FindAsync(id);
            if (feedback == null) {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", feedback.ProductId);
            return View(feedback);
        }

        // POST: Admin/Feedbacks/Edit/5 To protect from overposting attacks, enable the specific
        // properties you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FeedbackId,ProductId,Rank,Comment,Rate,CustomerId,isShow,FeedbackDay")] Feedback feedback) {
            if (id != feedback.FeedbackId) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(feedback);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!FeedbackExists(feedback.FeedbackId)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", feedback.ProductId);
            return View(feedback);
        }

        // GET: Admin/Feedbacks/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            var feedback = await _context.Feedback.FindAsync(id);
            _context.Feedback.Remove(feedback);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Feedbacks/Delete/5

        private bool FeedbackExists(int id) {
            return _context.Feedback.Any(e => e.FeedbackId == id);
        }
    }
}