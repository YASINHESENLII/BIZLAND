using BIZLAND.DAL;
using BIZLAND.Models;
using BIZLAND.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BIZLAND.Controllers
{
    public class HomeController : Controller
    {
        readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public  IActionResult Index()
        {
          var teams = _context.teams.ToList();
         HomeVM homeVM = new HomeVM
         {
             teams = teams,
         };
            return View(homeVM);
        }

      
    }
}