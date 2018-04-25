using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LevelStore.Models;
using LevelStore.Models.ViewModels;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LevelStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _repository;
        public AccountController(IUserRepository repo)
        {
            _repository = repo;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel{Email = "Bulankino@gmail.com", Password = "", RememberMe = true});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginData)
        {
            if (ModelState.IsValid)
            {
                bool result = await _repository.LogIn(loginData);
                
                if (result)
                {
                    await Authenticate(loginData.Email, loginData.RememberMe);
                    return RedirectToAction("ListAdmin", "Admin");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(loginData);
        }

        [Authorize]
        public IActionResult Register()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registrationData)
        {
            if (registrationData.Password != registrationData.ConfirmPassword)
            {
                ModelState.AddModelError("", "Пароли не совпадают!");
            }
            if (ModelState.IsValid)
            {
                bool result = await _repository.Registration(registrationData);
                
                if (result)
                {
                    await Authenticate(registrationData.Email);
                    return RedirectToAction("ListAdmin", "Admin");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль!");
            }
            return View(registrationData);
        }


        private async Task Authenticate(string userName, bool remember = false)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id), new AuthenticationProperties { IsPersistent = remember });
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Home");
        }
    }
}