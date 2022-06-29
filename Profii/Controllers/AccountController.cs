using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profii.Models;
using Profii.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Profii.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager  { get;  }
        private SignInManager<AppUser> _signInManager  { get; }
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager=signInManager;
    }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid) return View(register);
            AppUser newUser = new AppUser
            {
                FullName = register.FullName,
                Email = register.Email,
                UserName = register.UserName,
            };
            newUser.IsActivated= true;
            var identityResult = await _userManager.CreateAsync(newUser,register.Password);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(register);
            }
            await _signInManager.SignInAsync(newUser,true);
            return RedirectToAction("Index","Home");
        }
        public IActionResult CheckSignIn()
        {
            return Content(User.Identity.IsAuthenticated.ToString());
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM user)
        {
            AppUser userDb = await _userManager.FindByEmailAsync(user.Email);
            if (userDb == null)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(user);
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, false, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Please try a few minutes later");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email or password is wrong!");
                return View(user);
            }
            if (!userDb.IsActivated)
            {
                ModelState.AddModelError("", "Please verify ur account!");
                return View(user);
            }
            //ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            //return View("");

            return RedirectToAction("Index", "Account");
        }
        

    }
}
