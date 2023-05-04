﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using WEB2.Models;
using WEB2.Views.Shared.Components;

namespace WEB2.Areas.Identity.Pages.Account {

    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel {
        private readonly UserManager<AppUser> _userManager;

        public RegisterConfirmationModel( UserManager<AppUser> userManager ) {
            _userManager = userManager;
        }

        public string Email { get; set; }

        public string UrlContinue { set; get; }

        public async Task<IActionResult> OnGetAsync( string email, string returnUrl = null ) {
            if (email == null) {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) {
                return NotFound($"Không có user với email: '{email}'.");
            }

            if (user.EmailConfirmed) {
                // Tài khoản đã xác thực email
                return ViewComponent(MessagePage.COMPONENTNAME,
                        new MessagePage.Message() {
                            title = "Thông báo",
                            htmlcontent = "Tài khoản đã xác thực, chờ chuyển hướng",
                            urlredirect = (returnUrl != null) ? returnUrl : Url.Page("/Index")
                        }

                );
            }

            Email = email;

            if (returnUrl != null) {
                UrlContinue = Url.Page("RegisterConfirmation", new { email = Email, returnUrl = returnUrl });
            }
            else
                UrlContinue = Url.Page("RegisterConfirmation", new { email = Email });

            return Page();
        }
    }
}