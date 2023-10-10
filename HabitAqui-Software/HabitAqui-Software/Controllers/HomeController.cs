using HabitAqui_Software.Data;
using HabitAqui_Software.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO.Pipelines;

namespace HabitAqui_Software.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            ViewBag.Category = _context.Categories.ToList();

            IQueryable<Habitacao> query = _context.habitacaos
                           .Include(h => h.locador)
                           .Include(h => h.category);

            var results = await query.ToListAsync();

            return View(results);

        }
    
    //Initialize bag values using get (http protocol)
    public IActionResult Search()
    {
        ViewBag.Category = _context.Categories.ToList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Search(string? location, int? category, int? minimumRentalPeriod, string locador, DateTime? startDateAvailability, DateTime? endDateAvailability, string? price, string? grade)
    {
        ViewBag.Category = await _context.Categories.ToListAsync();
           
        IQueryable<Habitacao> query = _context.habitacaos
            .Include(h => h.locador)
            .Include(h => h.category)
            .Where(data => data.available == true);

        if (!string.IsNullOrEmpty(location))
        {
            query = query.Where(item => item.location.Contains(location));
        }

        if (category.HasValue && category.Value != 0)
        {
            query = query.Where(item => item.category.Id == category.Value);
        }

        if (minimumRentalPeriod.HasValue && minimumRentalPeriod.Value > 0)
        {
            query = query.Where(item => item.minimumRentalPeriod >= minimumRentalPeriod.Value);
        }

        if (!string.IsNullOrEmpty(locador))
        {
            query = query.Where(item => item.locador.name.Contains(locador));
        }

        if (startDateAvailability.HasValue)
        {
            query = query.Where(item => item.startDateAvailability >= startDateAvailability.Value);
        }

        if (endDateAvailability.HasValue)
        {
            query = query.Where(item => item.endDateAvailability <= endDateAvailability.Value);
        }

            switch (grade)
            {
                case "defaultGrade":
                    break;
                case "lowClassification":
                    query = query.OrderBy(item => item.grade);
                    break;
                case "highClassification":
                    query = query.OrderByDescending(item => item.grade);
                    break;
                default:
                    // Ordenação padrão, se nenhum critério for especificado.
                    break;
            }

            switch (price)
            {
                case "defaultPrice":
                    break;
                case "lowPrice":
                    query = query.OrderBy(item => item.rentalCost);
                    break;
                case "highPrice":
                    query = query.OrderByDescending(item => item.rentalCost);
                    break;
                default:
                    // Ordenação padrão, se nenhum critério for especificado.
                    break;
            }

            var results = await query.ToListAsync();

        return View("Index", results);
    }

     public IActionResult Order()
    {
        ViewBag.Category = _context.Categories.ToList();
        return View();
    }

    [HttpPost]

    public async Task<IActionResult> Order(string? price, string? grade)
    {
            ViewBag.Category = await _context.Categories.ToListAsync();

            IQueryable<Habitacao> query = _context.habitacaos.Include(h => h.locador).Include(h => h.category).Where(data => data.available == true);
           
            switch (grade)
            {
                case "defaultGrade":
                    break;
                case "lowClassification":
                    query = query.OrderBy(item => item.grade);
                    break;
                case "highClassification":
                    query = query.OrderByDescending(item => item.grade);
                    break;
                default:
                    // Ordenação padrão, se nenhum critério for especificado.
                    break;
            }

            switch (price)
            {
                case "defaultPrice":
                    break;
                case "lowPrice":
                    query = query.OrderBy(item => item.rentalCost);
                    break;
                case "highPrice":
                    query = query.OrderByDescending(item => item.rentalCost);
                    break;
                default:
                    // Ordenação padrão, se nenhum critério for especificado.
                    break;
            }

            

            var results = await query.ToListAsync();

            return View("Index", results);
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