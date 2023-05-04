using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WEB2.Areas.Admin.Pages.Manage {

    [Authorize("Admin")]
    public class ProfileModel : PageModel {

        public void OnGet() {
        }
    }
}