using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HabitAqui_Software.Data;
using HabitAqui_Software.Models;

namespace HabitAqui_Software.Controllers
{
    public class RentalContractsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private Locador _locador;

        public RentalContractsController(ApplicationDbContext context)
        {
            _context = context;

            //locador estatico de testes
            _locador = _context.locador.FirstOrDefault(l => l.Id == 7);
        }

        private string getLocadorName()
        {
            return _locador.name.ToString();
        }

        [HttpPost]
        public async Task<ActionResult> Search(DateTime? startDate, DateTime? endDate, Boolean? isConfirmed, string? location, string? cliente)
        {
            IQueryable<RentalContract> query = _context.rentalContracts.Include(r => r.habitacao).Where(l => l.habitacao.locador == _locador);

            if (startDate.HasValue)
            {
                query = query.Where(item => item.startDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(item => item.endDate <= endDate.Value);
            }

            if (isConfirmed.HasValue)
            {
                query = query.Where(item => item.isConfirmed == isConfirmed);
            }

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(item => item.habitacao.location.Contains(location));
            }

            if (!string.IsNullOrEmpty(cliente))
            {
                query = query.Where(item => item.user.UserName.Contains(cliente));
            }

            var result = await query.ToListAsync();

            ViewBag.locadorName = this.getLocadorName();

            return View("Index", result);
        }

        // GET: RentalContracts/Edit/5
        public async Task<IActionResult> Confirm(int? id)
        {
            if (id == null || _context.rentalContracts == null)
            {
                return NotFound();
            }

            var rentalContract = await _context.rentalContracts.FindAsync(id);
            if (rentalContract == null)
            {
                return NotFound();
            }

            //altera o atributo isConfirmed para true de modo a confirmar o arrendamento
            rentalContract.isConfirmed = true;

            //Guarda a alterção na bd
            _context.Entry(rentalContract).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return View(rentalContract);
        }

        // GET: RentalContracts
        public async Task<IActionResult> Index()
        {
            ViewBag.locadorName = this.getLocadorName();
            var applicationDbContext = _context.rentalContracts.Include(r => r.habitacao).Where(l => l.habitacao.locador == _locador);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RentalContracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.rentalContracts == null)
            {
                return NotFound();
            }

            var rentalContract = await _context.rentalContracts
                .Include(r => r.habitacao)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentalContract == null)
            {
                return NotFound();
            }

            return View(rentalContract);
        }

        // GET: RentalContracts/Create
        public IActionResult Create()
        {
            ViewData["HabitacaoId"] = new SelectList(_context.habitacaos, "Id", "Id");
            return View();
        }

        // POST: RentalContracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,startDate,endDate,isConfirmed,HabitacaoId,DeliveryStatusId,ReceiveStatusId,UserId")] RentalContract rentalContract)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rentalContract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HabitacaoId"] = new SelectList(_context.habitacaos, "Id", "Id", rentalContract.HabitacaoId);
            return View(rentalContract);
        }

        // GET: RentalContracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.rentalContracts == null)
            {
                return NotFound();
            }

            var rentalContract = await _context.rentalContracts.FindAsync(id);
            if (rentalContract == null)
            {
                return NotFound();
            }
            ViewData["HabitacaoId"] = new SelectList(_context.habitacaos, "Id", "Id", rentalContract.HabitacaoId);
            return View(rentalContract);
        }

        // POST: RentalContracts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,startDate,endDate,isConfirmed,HabitacaoId,DeliveryStatusId,ReceiveStatusId,UserId")] RentalContract rentalContract)
        {
            if (id != rentalContract.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rentalContract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalContractExists(rentalContract.Id))
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
            ViewData["HabitacaoId"] = new SelectList(_context.habitacaos, "Id", "Id", rentalContract.HabitacaoId);
            return View(rentalContract);
        }

        // GET: RentalContracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.rentalContracts == null)
            {
                return NotFound();
            }

            var rentalContract = await _context.rentalContracts
                .Include(r => r.habitacao)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentalContract == null)
            {
                return NotFound();
            }

            return View(rentalContract);
        }

        // POST: RentalContracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.rentalContracts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.rentalContracts'  is null.");
            }
            var rentalContract = await _context.rentalContracts.FindAsync(id);
            if (rentalContract != null)
            {
                _context.rentalContracts.Remove(rentalContract);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalContractExists(int id)
        {
          return (_context.rentalContracts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
