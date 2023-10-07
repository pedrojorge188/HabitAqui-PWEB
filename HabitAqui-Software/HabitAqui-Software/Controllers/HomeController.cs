using HabitAqui_Software.Data;
using HabitAqui_Software.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HabitAqui_Software.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;


        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Index(int? page)
        {

            int pageSize = 8;
            int pageNumber = page ?? 1;

            var habitacoesComLocador = _context.habitacaos.Include(h => h.locador).ToList();

            var habitacoesComCategoria = _context.habitacaos.Include(h => h.category).ToList();

            IQueryable<Habitacao> habitacaos = _context.habitacaos.Where(data => data.available == true);

            var model = habitacoesComLocador.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

    
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)habitacoesComLocador.Count() / pageSize);

            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {

            if (id == null || _context.habitacaos == null)
            {
                return NotFound();
            }


            var information = await _context.habitacaos.Include(h => h.locador).FirstOrDefaultAsync(m => m.Id == id);

            if (information == null)
            {
                return NotFound();
            }

            return View(information);
        }

        public IActionResult Credits()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}