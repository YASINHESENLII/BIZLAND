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
            AppUser userr = new AppUser
            {
                UserName=user.Usename,
                Email=user.Email,
                Name=user.Name,
                Surname =user.Surname,
            };

            IdentityResult error = await _usermanager.CreateAsync(userr,user.Password);
            if (error.Succeeded)
            {
                foreach (var item in error.Errors)
                {
                    ModelState.AddModelError(string.Empty,item.Description);
                    return View();
                }
            }
            await _signInManager.SignInAsync(userr, false);

            return RedirectToAction("Index","Home");




        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult>Login(LoginVM login)
        {
            AppUser existed = await _usermanager.FindByEmailAsync(login.UsernameOrEmail);
            if(existed == null)
            {
                existed=await _usermanager.FindByNameAsync(login.UsernameOrEmail);
                if( existed == null)
                {
                    ModelState.AddModelError(string.Empty, "Bele istifadeci movcuddur");
                    return View();
                }
            }
            var asa = await _signInManager.PasswordSignInAsync(existed, login.Password, login.RememberMe, false);
            if(asa == null)
            {

                ModelState.AddModelError(string.Empty, "Bele istifadeci movcuddur");
                return View();
            }



            return RedirectToAction("Index", "Home");
        }


    }
}
