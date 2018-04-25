using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LevelStore.Models.ViewModels;

namespace LevelStore.Models
{
    public interface IUserRepository
    {
        IEnumerable<User> Users { get; }
        Task<bool> LogIn(LoginViewModel loginData);
        Task<bool> Registration(RegisterViewModel registrationData);
    }
}
