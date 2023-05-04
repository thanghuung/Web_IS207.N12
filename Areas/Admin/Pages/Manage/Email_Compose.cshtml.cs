using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WEB2.Areas.Admin.Pages.Manage {

    [Authorize("Admin")]
    public class Email_ComposeModel : PageModel {

        public void OnGet() {
        }
    }
}