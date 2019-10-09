using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Solar.Models;
using Solar.Repositories;

namespace Solar.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly SolarDbContext repository;

        public AuthenticationService(SolarDbContext repository)
        {
            this.repository = repository;
        }

        public async Task<string> GetNameByEmail(string email)
        {
            var user = await repository.Users.Where(u => u.Email.Equals(email)).FirstOrDefaultAsync();
            return user.Name;
        }

        public async Task<bool> LogIn(string user, string password)
        {
            return await repository.Users.AnyAsync(u => u.Email.Equals(user) && u.Password.Equals(password));
        }

        public async Task<bool> SignUp(User user)
        {
            if (user == null)
                return false;
            await repository.Users.AddAsync(user);
            await repository.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UserExist(string email)
        {
            return await repository.Users.AnyAsync(u => u.Email.Equals(email));
        }
    }
}
