using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HabitAqui_Software.Data;
using HabitAqui_Software.Models;
using Microsoft.AspNetCore.Identity;

namespace HabitAqui_Software.Controllers
{
    public class ParqueHabitacoesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private Locador _locador;

        public ParqueHabitacoesController(ApplicationDbContext context)
        {
            _context = context;

            //locador estatico de testes
            _locador = _context.locador.FirstOrDefault(l => l.Id == 6);
  
        }

       private string getLocadorName()
        {
            return _locador.name.ToString();
        }

        // GET: ParqueHabitacoes
        public async Task<IActionResult> Index()
        {
            ViewBag.locadorName = this.getLocadorName();

            var cat = _context.Categories.ToList();
            ViewBag.cat = cat;
            ViewBag.Category = new SelectList(cat, "Id", "name");

            return _context.habitacaos != null ?
                        View(await _context.habitacaos.Include(it => it.category).Where(l => l.locador == _locador)
                        .ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.habitacaos'  is null.");
        }

        [HttpPost]
        public async Task<ActionResult> Search(Boolean? disp, int? category, string? price, string? grade)
        {   
            IQueryable<Habitacao> query = _context.habitacaos.Include(c => c.category)
                .Where(l => l.locador == _locador);

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

            habitacao.locador = _locador;
            habitacao.grade = 0;
       
            _context.Add(habitacao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
 
        }

        // GET: ParqueHabitacoes/Edit/5
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,location,rentalCost,startDateAvailability,endDateAvailability,minimumRentalPeriod,maximumRentalPeriod,available,grade,categoryId")] Habitacao habitacao)
        {

            if (id != habitacao.Id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(habitacao);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HabitacaoExists(habitacao.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: ParqueHabitacoes/Delete/5
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
