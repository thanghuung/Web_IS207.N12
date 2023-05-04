using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace WEB2.Infrastructure
{
    public class BaseController : Controller
    {
        protected string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
