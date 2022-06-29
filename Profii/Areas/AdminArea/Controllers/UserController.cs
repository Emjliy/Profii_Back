using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profii.DAL;
using Profii.Models;
using Profii.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Profii.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]

    public class UserController : Controller
    {
        private AppDbContext _context  { get;  }
        private SignInManager<AppUser> _signInManager { get; }
        private UserManager<AppUser> _userManager { get; }
        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(LoginVM user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser newUser = new AppUser
            {
                UserName = user.UserName,
                Email = user.Email,
            };
            var identityResult =await _userManager.CreateAsync(newUser, user.Password);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(user);
            }
            await _signInManager.SignInAsync(newUser, true);
            return View(user);
        }
            public async Task<IActionResult> Login(LoginVM user) {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, true, false);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(user);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
    }
}
