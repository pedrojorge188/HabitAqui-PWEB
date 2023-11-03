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
        private UserTeste _userTeste;

        public RentalContractsController(ApplicationDbContext context)
        {
            _context = context;

            //locador estatico de testes
            _locador = _context.locador.FirstOrDefault(l => l.Id == 7);

            //userTeste -------------------------------------------------------------------
            _userTeste = _context.userTeste.FirstOrDefault(u => u.Id == 1);
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
 
            var rentalContract = await _context.rentalContracts.FindAsync(id);
            if (rentalContract == null)
            {
                return NotFound();
            }

            rentalContract.isConfirmed = true;
           
            _context.Update(rentalContract);
            await _context.SaveChangesAsync();
            
            return View("RentalContracts",rentalContract);
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





        //-----------------------------------------------RENTALCONTRACTS USER/CLIENTE----------------------------------------------------
        public async Task<IActionResult> History()
        {
            ViewBag.Category = _context.Categories.ToList();

            IQueryable<RentalContract> query = _context.rentalContracts
                           .Include(h => h.habitacao)
                           .Where(i => i.userTeste == this._userTeste);
            ;


            var results = await query.ToListAsync();

            return View(results);

        }


        
        public IActionResult CreateClienteAval(int IdHabitacao)
        {
            var ren = _context.rentalContracts.ToList();
            ViewBag.ren = ren;
            ViewBag.RentalContracts = new SelectList(ren, "Id", "avaliacao");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClienteAval(int IdHabitacao, [Bind("Id,startDate," +
            "endDate,isConfirmed,avaliacao,")] RentalContract rentalContract)
        {
            var ren = _context.rentalContracts.ToList();
            ViewBag.ren = ren;
            ViewBag.RentalContracts = new SelectList(ren, "Id", "avaliacao");

            rentalContract.HabitacaoId = IdHabitacao;
            rentalContract.userTeste = _userTeste;
            

            if (ModelState.IsValid)
            {
                _context.Add(rentalContract);
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

        
        public async Task<IActionResult> EditClienteAval(int? id)
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
    }
}
