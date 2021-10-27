using ChatApplication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ChatApplication.Domain.Services
{
    public interface IAccountService
    {
        Task<SignInResult> Login(string userName, string password);

        Task<User> Register(string userName, string password);

        Task Logout();
    }
}
