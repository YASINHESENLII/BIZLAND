using BIZLAND.Models;
using BIZLAND.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BIZLAND.Controllers
{
    public class AccauntController : Controller
    {
        readonly UserManager<AppUser> _usermanager;
        readonly SignInManager<AppUser> _signInManager;

        public AccauntController(UserManager<AppUser> usermanager, SignInManager<AppUser> signInManager)
        {
            _usermanager = usermanager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Register(RegisterVM user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser exis = new AppUser
            {
                Name = user.Name,
                Email = user.Email,
                Surname = user.Surname,
                UserName=user.Usename
                

            };
            IdentityResult result = await _usermanager.CreateAsync(exis,user.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                    return View();
                    
                }
            }

          await  _signInManager.SignInAsync(exis,false);

            return RedirectToAction("Index","Home");
     }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index","Home");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult>Login(LoginVM login)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var exs = await _usermanager.FindByEmailAsync(login.UsernameOrEmail);
            if(exs == null)
            {
                exs = await _usermanager.FindByNameAsync(login.UsernameOrEmail);
                if (exs == null)
                {
                    ModelState.AddModelError(String.Empty, "Bu adda istifadexi yoxdur");

                }
            }

          var pas =  await _signInManager.PasswordSignInAsync(exs, login.Password, login.RememberMe, false);

            if (pas == null)
            {

                ModelState.AddModelError(string.Empty, "Bu adda username ve email tapilmadi");
                return View();
            }

            return RedirectToAction("Index", "Home");

        }


    }
}
