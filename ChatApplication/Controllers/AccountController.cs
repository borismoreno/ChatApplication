using System.Threading.Tasks;
using ChatApplication.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.Controllers
{
    public class AccountController : Controller
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            
            var result = await _accountService.Login(userName, password);

            if (result != null && result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string userName, string password)
        {
            var result = await _accountService.Register(userName, password);
            if(result != null)
            {
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("Register", "Account");
        }

        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return RedirectToAction("Login", "Account");
        }
    }
}