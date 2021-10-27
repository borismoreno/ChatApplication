using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.Controllers
{
    public class BaseController : Controller
    {
        protected string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        protected string GetUserName()
        {
            return User.Identity.Name;
        }
    }
}