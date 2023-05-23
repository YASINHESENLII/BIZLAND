using BIZLAND.DAL;
using BIZLAND.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BIZLAND.Areas.Areas.Controllers
{
    [Area("Admin")]
    public class TeamController : Controller
    {
        

        readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;
        public TeamController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

       

        public IActionResult Index()
        {
            List<Team> teams = _context.teams.ToList();

            return View(teams);
        }

      public IActionResult Create()
        {
            return View();  
        }

        [HttpPost]

        public async Task<IActionResult> Create(Team team)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var a = await _context.teams.FirstOrDefaultAsync(t => t.Id == team.Id);

            if(a != null)
            {
                return View();
            }

            if(!team.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "sekilqoy");
                return View();
            }
            if (team.Photo.Length > 200 * 1024)
            {
                ModelState.AddModelError("Photo", "olcu");
                return View();
            }

            string filename  = Guid.NewGuid().ToString() + team.Photo.FileName;

            string path = Path.Combine(_env.WebRootPath, "assets/img", filename);
            using (FileStream file = new FileStream(path,FileMode.Create))
            {
                await team.Photo.CopyToAsync(file);
            }
            team.Image = filename;

            await _context.teams.AddAsync(team);    
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
            
        }

        public async Task<IActionResult> Delete(int? id)
        {
            Team existed = await _context.teams.FirstOrDefaultAsync(x=>x.Id == id);
            if (existed == null)
            {

                return View();
            }

            string path = Path.Combine(_env.WebRootPath, "assets/img", existed.Image);

            if(!System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            _context.teams.Remove(existed);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public IActionResult Update()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Update(Team team)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Team existed = await _context.teams.FirstOrDefaultAsync(x=>x.Id==team.Id);
            if (existed != null)
            {
                if (!team.Photo.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("Photo", "sekilqoy");
                    return View();
                }
                if (team.Photo.Length > 200 * 1024)
                {
                    ModelState.AddModelError("Photo", "olcu");
                    return View();
                }

                string path  = Path.Combine(_env.WebRootPath,"assets/img",existed.Image);
              
                System.IO.File.Delete(path);
                
                string newfilename= Guid.NewGuid().ToString() + team.Photo.FileName;

                string newpath = Path.Combine(_env.WebRootPath,"assets/img",newfilename);
                using (FileStream file = new FileStream(newpath,FileMode.Create))
                {
                    await team.Photo.CopyToAsync(file);
                }
                existed.Image = newfilename;

            }
            existed.Name= team.Name;
            existed.Title= team.Title;

         
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");



        }


    }
}
