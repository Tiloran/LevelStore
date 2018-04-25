using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LevelStore.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LevelStore.Models.EF
{
    public class EFUserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;

        public EFUserRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<User> Users { get; }

        public async Task<bool> LogIn(LoginViewModel loginData)
        {
            User user = await context.Users.FirstOrDefaultAsync(u => u.Email == loginData.Email && u.Password == loginData.Password);
            if (user != null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> Registration(RegisterViewModel registrationData)
        {
            User user = await context.Users.FirstOrDefaultAsync(u => u.Email == registrationData.Email);
            if (user == null)
            {
                // добавляем пользователя в бд
                context.Users.Add(new User { Email = registrationData.Email, Password = registrationData.Password });
                await context.SaveChangesAsync();
                return true;
            }

            return false;
        }


        
    }
}
