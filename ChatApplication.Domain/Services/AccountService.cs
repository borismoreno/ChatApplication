using ChatApplication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ChatApplication.Domain.Services
{
    public class AccountService : IAccountService
    {

        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<SignInResult> Login(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                return await _signInManager.PasswordSignInAsync(user, password, false, false);
            }
            return null;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<User> Register(string userName, string password)
        {
            var user = new User
            {
                UserName = userName
            };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return user;
            }
            return null;
        }
    }
}
