using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB2.Data;
using WEB2.Models;

namespace WEB2.Areas.Admin.Controllers {

    [Area("Admin")]
    [Authorize("Admin")]
    public class StaffsController : Controller {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public StaffsController(AppDbContext context
            , UserManager<AppUser> userManager) {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/Staffs
        public async Task<IActionResult> Index() {
            var appDbContext = _context.Staff.Include(s => s.AppUser);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/Staffs/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var staff = await _context.Staff
                .Include(s => s.AppUser)

                .FirstOrDefaultAsync(m => m.StaffId == id);
            if (staff == null) {
                return NotFound();
            }

            return View(staff);
        }

        // GET: Admin/Staffs/Create
        public IActionResult Create() {
            ViewData["InventoryId"] = new SelectList(_context.Set<Inventory>(), "InventoryId", "InventoryId");
            return View();
        }

        // POST: Admin/Staffs/Create To protect from overposting attacks, enable the specific
        // properties you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppUser")] Staff staff) {
            if (ModelState.IsValid) {
                //tạo user
                var user = new AppUser {
                    UserName = staff.AppUser.UserName,
                    Email = staff.AppUser.Email,
                    FullName = staff.AppUser.FullName,
                    PhoneNumber = staff.AppUser.PhoneNumber,
                    Birthday = staff.AppUser.Birthday,
                    EmailConfirmed = true
                };
                var pass = staff.AppUser.PasswordHash;
                var result = await _userManager.CreateAsync(user, pass);
                //tạo nhân viên
                var staffnew = new Staff { UserId = user.Id, WorkingDay = DateTime.Now };
                _context.Add(staffnew);
                //add roles
                var role = await _context.Roles.FirstOrDefaultAsync(p => p.Name.Equals("Nhân viên"));
                await _userManager.AddToRoleAsync(user, role.Name);
                await _context.SaveChangesAsync();
                //thêm tên nhân viên

                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", staff.UserId);

            return View(staff);
        }

        // GET: Admin/Staffs/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var staff = await _context.Staff.FindAsync(id);
            if (staff == null) {
                return NotFound();
            }

            return View(staff);
        }

        // POST: Admin/Staffs/Edit/5 To protect from overposting attacks, enable the specific
        // properties you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StaffId,InventoryId,UserId,WorkingDay")] Staff staff) {
            if (id != staff.StaffId) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(staff);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!StaffExists(staff.StaffId)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", staff.UserId);

            return View(staff);
        }

        // GET: Admin/Staffs/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var staff = await _context.Staff
                .Include(s => s.AppUser)

                .FirstOrDefaultAsync(m => m.StaffId == id);
            if (staff == null) {
                return NotFound();
            }

            return View(staff);
        }

        // POST: Admin/Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var staff = await _context.Staff.FindAsync(id);
            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffExists(int id) {
            return _context.Staff.Any(e => e.StaffId == id);
        }
    }
}