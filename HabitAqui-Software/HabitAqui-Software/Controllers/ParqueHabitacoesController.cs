using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HabitAqui_Software.Data;
using HabitAqui_Software.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;



namespace HabitAqui_Software.Controllers {

    [Authorize(Roles = "Employer, Manager")]
    public class ParqueHabitacoesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser _currentUser;

        public ParqueHabitacoesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        private string getLocadorName()
        {

            if (User.IsInRole("Employer"))
            {
                var appUserId = _userManager.GetUserId(User);
                var employee = _context.employers.Where(uid => uid.user.Id == appUserId).FirstOrDefault();
                var _locador = _context.locador.FirstOrDefault(l => l.Id == employee.LocadorId);
                return _locador.name;

            }
            else if (User.IsInRole("Manager"))
            {
                var appUserId = _userManager.GetUserId(User);
                var manager = _context.managers.Where(uid => uid.user.Id == appUserId).FirstOrDefault();
                var _locador = _context.locador.FirstOrDefault(l => l.Id == manager.LocadorId);

                return _locador.name;
            }
            return " ";
        }

        [Authorize(Roles = "Employer, Manager")]
        // GET: ParqueHabitacoes
        public async Task<IActionResult> Index()
        {
            ViewBag.locadorName = this.getLocadorName();

            var cat = _context.Categories.ToList();
            ViewBag.cat = cat;
            ViewBag.Category = new SelectList(cat, "Id", "name");

            if (User.IsInRole("Employer"))
            {
                var appUserId = _userManager.GetUserId(User);
                var employee = _context.employers.Where(uid => uid.user.Id == appUserId).FirstOrDefault();
                var _locador = _context.locador.FirstOrDefault(l => l.Id == employee.LocadorId);

                return _context.habitacaos != null ?
                     View(await _context.habitacaos.Include(it => it.category).Where(l => l.locador == _locador)
                     .ToListAsync()) :
                     Problem("Entity set 'ApplicationDbContext.habitacaos'  is null.");

            }
            else if (User.IsInRole("Manager"))
            {
                var appUserId = _userManager.GetUserId(User);
                var manager = _context.managers.Where(uid => uid.user.Id == appUserId).FirstOrDefault();
                var _locador = _context.locador.FirstOrDefault(l => l.Id == manager.LocadorId);

                return _context.habitacaos != null ?
                     View(await _context.habitacaos.Include(it => it.category).Where(l => l.locador == _locador)
                     .ToListAsync()) :
                     Problem("Entity set 'ApplicationDbContext.habitacaos'  is null.");
            }

            return null;
        }

        [Authorize(Roles = "Employer, Manager")]
        [HttpPost]
        public async Task<ActionResult> Search(Boolean? disp, int? category, string? price, string? grade) {

            IQueryable<Habitacao> query = null;

            if (User.IsInRole("Employer"))
            {
                var appUserId = _userManager.GetUserId(User);
                var employee = _context.employers.Where(uid => uid.user.Id == appUserId).FirstOrDefault();
                var _locador = _context.locador.FirstOrDefault(l => l.Id == employee.LocadorId);
                query = _context.habitacaos.Include(c => c.category)
               .Where(l => l.locador == _locador);

            }
            else if (User.IsInRole("Manager"))
            {
                var appUserId = _userManager.GetUserId(User);
                var manager = _context.managers.Where(uid => uid.user.Id == appUserId).FirstOrDefault();
                var _locador = _context.locador.FirstOrDefault(l => l.Id == manager.LocadorId);
                query = _context.habitacaos.Include(c => c.category)
               .Where(l => l.locador == _locador);
            }

            if (disp.HasValue)
            {
                query = query.Where(it => it.available == disp);
            }

            if (category.HasValue && category.Value != 0)
            {   
       
                query = query.Where(it => it.category.Id == category);
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
                    break;
            }

            var result = await query.ToListAsync();

            ViewBag.locadorName = this.getLocadorName();
            var cat = _context.Categories.ToList();
            ViewBag.cat = cat;
            ViewBag.Category = new SelectList(cat, "Id", "name");

            return View("Index", result);
        }

        [Authorize(Roles = "Employer, Manager")]
        // GET: ParqueHabitacoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var cat = _context.Categories.ToList();
            ViewBag.cat = cat;
            ViewBag.Category = new SelectList(cat, "Id", "name");

            if (id == null || _context.habitacaos == null)
            {
                return NotFound();
            }

            var habitacao = await _context.habitacaos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (habitacao == null)
            {
                return NotFound();
            }

            return View(habitacao);
        }

        [Authorize(Roles = "Employer, Manager")]
        // GET: ParqueHabitacoes/Create
        public IActionResult Create()
        {
            var cat = _context.Categories.ToList();
            ViewBag.cat = cat;
            ViewBag.Category = new SelectList(cat, "Id", "name");
            return View();
        }

        // POST: ParqueHabitacoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Employer, Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,location," +
            "rentalCost,startDateAvailability,endDateAvailability," +
            "minimumRentalPeriod,maximumRentalPeriod," +
            "available,categoryId")] Habitacao habitacao)
        {
            var cat = _context.Categories.ToList();
            ViewBag.cat = cat;
            ViewBag.Category = new SelectList(cat, "Id", "name");

            if (User.IsInRole("Employer"))
            {
                var appUserId = _userManager.GetUserId(User);
                var employee = _context.employers.Where(uid => uid.user.Id == appUserId).FirstOrDefault();
                var _locador = _context.locador.FirstOrDefault(l => l.Id == employee.LocadorId);

                habitacao.locador = _locador;
                habitacao.grade = 0;

            }else if (User.IsInRole("Manager"))
            {
                var appUserId = _userManager.GetUserId(User);
                var manager = _context.managers.Where(uid => uid.user.Id == appUserId).FirstOrDefault();
                var _locador = _context.locador.FirstOrDefault(l => l.Id == manager.LocadorId);

                habitacao.locador = _locador;
                habitacao.grade = 0;
            }
         
       
            _context.Add(habitacao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
 
        }

        // GET: ParqueHabitacoes/Edit/5
        [Authorize(Roles = "Employer, Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            var cat = _context.Categories.ToList();
            ViewBag.cat = cat;
            ViewBag.Category = new SelectList(cat, "Id", "name");

            if (id == null || _context.habitacaos == null)
            {
                return NotFound();
            }

            var habitacao = await _context.habitacaos.FindAsync(id);
            if (habitacao == null)
            {
                return NotFound();
            }
            return View(habitacao);
        }

        // POST: ParqueHabitacoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employer, Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,location,rentalCost,startDateAvailability,endDateAvailability,minimumRentalPeriod,maximumRentalPeriod,available,grade,LocadorId,categoryId")] Habitacao habitacao)
        {

            if (id != habitacao.Id)
            {
                return NotFound();
            }


            if (User.IsInRole("Employer"))
            {
                var appUserId = _userManager.GetUserId(User);
                var employee = _context.employers.Where(uid => uid.user.Id == appUserId).FirstOrDefault();
                var _locador = _context.locador.FirstOrDefault(l => l.Id == employee.LocadorId);

                habitacao.LocadorId = _locador.Id;

            }
            else if (User.IsInRole("Manager"))
            {
                var appUserId = _userManager.GetUserId(User);
                var manager = _context.managers.Where(uid => uid.user.Id == appUserId).FirstOrDefault();
                var _locador = _context.locador.FirstOrDefault(l => l.Id == manager.LocadorId);

                habitacao.LocadorId = _locador.Id;
 
            }

    

            if (ModelState.IsValid)
            {

                _context.Update(habitacao);
                await _context.SaveChangesAsync();
                
            }
            else
            {
                // Coleta as mensagens de erro do ModelState
                var erros = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);

                // Exibe as mensagens de erro no console
                foreach (var erro in erros)
                {
                    Console.WriteLine("Erro de validação: " + erro);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: ParqueHabitacoes/Delete/5
        [Authorize(Roles = "Employer, Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.habitacaos == null)
            {
                return NotFound();
            }

            var habitacao = await _context.habitacaos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (habitacao == null)
            {
                return NotFound();
            }

            return View(habitacao);
        }

        // POST: ParqueHabitacoes/Delete/5
        [Authorize(Roles = "Employer, Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.habitacaos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.habitacaos'  is null.");
            }
            var habitacao = await _context.habitacaos.FindAsync(id);
            if (habitacao != null)
            {
                _context.habitacaos.Remove(habitacao);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HabitacaoExists(int id)
        {
            return (_context.habitacaos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
