using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB2.Data;
using WEB2.Models;

namespace WEB2.Areas.Identity.Pages.Account.Manage {

    [Authorize]
    public class BillModel : PageModel {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public BillModel(AppDbContext context,
             UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager) {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IList<OrderDetail> orderdetail { get; set; }

        public async Task<IActionResult> OnGetAsync() {
            var user = await _userManager.GetUserAsync(User);
            var userName = await _userManager.GetUserNameAsync(user);
            var customer = await _userManager.GetUserIdAsync(user);

            var customerid = await _context.Customer.Include(p => p.AppUser)
                .Where(p => p.UserId == customer).FirstOrDefaultAsync();
            var order = await _context.Order.Include(p => p.Payment)
                .Include(p => p.Shipment)
                .Include(p => p.Customer)
                .Include(p => p.Customer.AppUser)
                .Where(p => p.CustomerId == customerid.CustomerID)
                 .OrderByDescending(p => p.OrderId)
                .ToListAsync();
            orderdetail = new List<OrderDetail>();
            foreach (var item in order) {
                var OrderDetail = await _context.OrderDetail
                .Include(o => o.Order)
                .Include(o => o.Product)
                .Include(o => o.Order.Customer)
                .Include(o => o.Order.Customer.AppUser)
                .Where(o => o.OrderId == item.OrderId)
                .Where(o => o.Status == "solved")
                .ToListAsync();
                foreach (var pro in OrderDetail) {
                    orderdetail.Add(pro);
                }
            }
            if (orderdetail == null) {
                return NotFound();
            }
            return Page();
        }
    }
}