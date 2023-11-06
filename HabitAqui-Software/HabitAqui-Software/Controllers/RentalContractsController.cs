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

namespace HabitAqui_Software.Controllers
{

    [Authorize(Roles = "Client, Employer, Manager")]
    public class RentalContractsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RentalContractsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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
        [HttpPost]
        public async Task<ActionResult> Search(DateTime? startDate, DateTime? endDate, Boolean? isConfirmed, string? location, string? cliente)
        {
            IQueryable<RentalContract> query = null;

            if (User.IsInRole("Employer"))
            {
                var appUserId = _userManager.GetUserId(User);
                var employee = _context.employers.Where(uid => uid.user.Id == appUserId).FirstOrDefault();
                var _locador = _context.locador.FirstOrDefault(l => l.Id == employee.LocadorId);
                query = _context.rentalContracts.Include(r => r.habitacao).Where(l => l.habitacao.locador == _locador);

            }
            else if (User.IsInRole("Manager"))
            {
                var appUserId = _userManager.GetUserId(User);
                var manager = _context.managers.Where(uid => uid.user.Id == appUserId).FirstOrDefault();
                var _locador = _context.locador.FirstOrDefault(l => l.Id == manager.LocadorId);
                query = _context.rentalContracts.Include(r => r.habitacao).Where(l => l.habitacao.locador == _locador);
            }
 

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

        [Authorize(Roles = "Employer, Manager")]
        // GET: RentalContracts
        public async Task<IActionResult> Index()
        {
            ViewBag.locadorName = this.getLocadorName();

            if (User.IsInRole("Employer"))
            {
                var appUserId = _userManager.GetUserId(User);
                var employee = _context.employers.Where(uid => uid.user.Id == appUserId).FirstOrDefault();
                var _locador = _context.locador.FirstOrDefault(l => l.Id == employee.LocadorId);
                var applicationDbContext = _context.rentalContracts.Include(r => r.habitacao).Where(l => l.habitacao.locador == _locador);
                return View(await applicationDbContext.ToListAsync());

            }
            else if (User.IsInRole("Manager"))
            {
                var appUserId = _userManager.GetUserId(User);
                var manager = _context.managers.Where(uid => uid.user.Id == appUserId).FirstOrDefault();
                var _locador = _context.locador.FirstOrDefault(l => l.Id == manager.LocadorId);
                var applicationDbContext = _context.rentalContracts.Include(r => r.habitacao).Where(l => l.habitacao.locador == _locador);
                return View(await applicationDbContext.ToListAsync());
            }
            return View();
        
        }
        [Authorize(Roles = "Employer, Manager")]
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
        [Authorize(Roles = "Employer, Manager")]
        public IActionResult Create()
        {
            ViewData["HabitacaoId"] = new SelectList(_context.habitacaos, "Id", "Id");
            return View();
        }

        // POST: RentalContracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Employer, Manager")]
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
        [Authorize(Roles = "Employer, Manager")]
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
        [Authorize(Roles = "Employer, Manager")]
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
        [Authorize(Roles = "Employer, Manager")]
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
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> History()
        {
            var appUserId = _userManager.GetUserId(User);
            IQueryable<RentalContract> rentalContracts = _context.rentalContracts.Include("habitacao").Where(ren => ren.user.Id == appUserId &&
                                                                                                             ren.isConfirmed == true);

            return View(await rentalContracts.ToListAsync());
        }


        [Authorize(Roles = "Client")]
        public async Task<IActionResult> EditClienteAval(int? id)
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
            return View(rentalContract);
        }


        // POST: Curso/Edit/5
        [Authorize(Roles = "Client")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditClienteAval(int? id, RentalContract rentalContract)
        {
            if (id == null || _context.rentalContracts == null)
            {
                return NotFound();
            }
            
            var grade = rentalContract.avaliacao;

            RentalContract rc = await _context.rentalContracts.FindAsync(id);
            rc.avaliacao = grade;

            if (ModelState.IsValid)
            {
                _context.Update(rc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(History));
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
            return View();
        }
    }
}
