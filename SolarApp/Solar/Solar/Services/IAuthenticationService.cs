using Solar.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Solar.Services
{
    public interface IAuthenticationService
    {
        Task<bool> LogIn(string user, string password);
        Task<bool> SignUp(User user);
        Task<bool> UserExist(string email);
        Task<string> GetNameByEmail(string email);
        Task<User> GetUserIdByEmail(string email);
    }
}
