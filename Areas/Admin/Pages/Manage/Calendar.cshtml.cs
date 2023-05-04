using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEB2.Data;
using WEB2.Models;

namespace WEB2.Areas.Admin.Pages.Manage {

    [Authorize("Admin")]
    public class CalendarModel : PageModel {
        private readonly AppDbContext _context;

        public CalendarModel( AppDbContext context ) {
            _context = context;
        }

        public string ErrorMessage { get; private set; }

        public IList<Calendar> Calendars { get; set; }

        public async Task OnGetAsync() {
            Calendars = await _context.Calendar.AsNoTracking()
                .ToListAsync();
        }

        //public async Task<IActionResult> OnPostAsync( string returnUrl = null ) {
        //    returnUrl ??= Url.Content("~/");
        //    // Đã đăng nhập nên chuyển hướng về Index
        //    if (_signInManager.IsSignedIn(User))
        //        return Redirect("Index");

        //    if (ModelState.IsValid) {
        //        // Thử login bằng username/password
        //        var result = await _signInManager.PasswordSignInAsync(
        //            Input.UserNameOrEmail,
        //            Input.Password,
        //            Input.RememberMe,
        //            true
        //        );

        //        if (!result.Succeeded) {
        //            // Thất bại username/password -> tìm user theo email, nếu thấy thì thử đăng nhập
        //            // bằng user tìm được
        //            var user = await _userManager.FindByEmailAsync(Input.UserNameOrEmail);
        //            if (user != null) {
        //                result = await _signInManager.PasswordSignInAsync(
        //                    user,
        //                    Input.Password,
        //                    Input.RememberMe,
        //                    true
        //                );
        //            }
        //        }

        //        if (result.Succeeded) {
        //            _logger.LogInformation("User logged in.");
        //            return LocalRedirect(returnUrl);
        //        }
        //        if (result.RequiresTwoFactor) {
        //            // Nếu cấu hình đăng nhập hai yếu tố thì chuyển hướng đến LoginWith2fa
        //            return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
        //        }
        //        if (result.IsLockedOut) {
        //            _logger.LogWarning("Tài khoản bí tạm khóa.");
        //            // Chuyển hướng đến trang Lockout - hiện thị thông báo
        //            return RedirectToPage("./Lockout");
        //        }
        //        else {
        //            ModelState.AddModelError(string.Empty, "Không đăng nhập được.");
        //            return Page();
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return Page();
        //}
    }
}