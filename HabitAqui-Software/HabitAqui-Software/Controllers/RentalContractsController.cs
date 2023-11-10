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
using SQLitePCL;
using HabitAqui_Software.Models.ViewModels;
using System.Diagnostics;

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
                var applicationDbContext = _context.rentalContracts.Include(r => r.habitacao).Include(u => u.user).Where(l => l.habitacao.locador == _locador);
                return View(await applicationDbContext.ToListAsync());

            }
            else if (User.IsInRole("Manager"))
            {
                var appUserId = _userManager.GetUserId(User);
                var manager = _context.managers.Where(uid => uid.user.Id == appUserId).FirstOrDefault();
                var _locador = _context.locador.FirstOrDefault(l => l.Id == manager.LocadorId);
                var applicationDbContext = _context.rentalContracts.Include(r => r.habitacao).Include(u => u.user).Where(l => l.habitacao.locador == _locador);
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

            var habitationsId = _context.rentalContracts.Select(rc => rc.HabitacaoId).ToList();
            var locadorIds = _context.rentalContracts.Where(rc => habitationsId.Contains(rc.HabitacaoId)).Select(rc => rc.habitacao.LocadorId).ToList();
            var habitationList = _context.habitacaos
                .Where(av => av.available == true && !habitationsId.Contains(av.Id) && locadorIds.Contains(av.LocadorId))
                .ToList();
            var users = _userManager.GetUsersInRoleAsync("Client").Result.ToList();
            ViewData["HabitacaoId"] = new SelectList(habitationList, "Id", "location");
            ViewData["userId"] = new SelectList(users, "Id", "firstName");

            return View();
        }


        [Authorize(Roles = "Employer, Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,startDate,endDate,isConfirmed,HabitacaoId,DeliveryStatusId,ReceiveStatusId,userId")] RentalContract rentalContract)
        {
            if (ModelState.IsValid)
            {
                ViewBag.SuccessMessage = "Contrato criado com sucesso";
                rentalContract.avaliacao = 0;
                rentalContract.isConfirmed = false;
               
                _context.Add(rentalContract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var habitationsId = _context.rentalContracts.Where(rc => rc.isConfirmed == false).Select(rc => rc.HabitacaoId).ToList();
            var locadorIds = _context.rentalContracts.Where(rc => habitationsId.Contains(rc.HabitacaoId)).Select(rc => rc.habitacao.LocadorId).ToList();
            var habitationList = _context.habitacaos
                .Where(av => av.available == true && !habitationsId.Contains(av.Id) && locadorIds.Contains(av.LocadorId))
                .ToList();
            var users = _userManager.GetUsersInRoleAsync("Client").Result.ToList();
            ViewData["HabitacaoId"] = new SelectList(habitationList, "Id", "location");
            ViewData["userId"] = new SelectList(users, "Id", "firstName");

            return View(rentalContract);
        }

      
        [Authorize(Roles = "Employer, Manager")]
        public async Task<IActionResult> Confirm(int? id)
        {
            if (id == null || _context.rentalContracts == null)
            {
                return NotFound();
            }

            ConfirmRentalContracts crc = new ConfirmRentalContracts();
            crc.rentalContract =  await _context.rentalContracts.FindAsync(id);
              
            if (crc.rentalContract == null)
            {
                return NotFound();
            }
            return View(crc);
        }


        [Authorize(Roles = "Employer, Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(int id, ConfirmRentalContracts confirmRentalContracts)
        {
            if (confirmRentalContracts.rentalContract == null || id != confirmRentalContracts.rentalContract.Id)
            {
                return Problem("Entity set 'ApplicationDbContext.rentalContracts'  is null.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    DeliveryStatus delivery = new DeliveryStatus
                    {
                        hasDamage = confirmRentalContracts.hasDamage,
                        hasEquipments = confirmRentalContracts.hasEquipments,
                        observation = confirmRentalContracts.observation
                    };

                    _context.Add(delivery);
                    await _context.SaveChangesAsync();


                    RentalContract rc = await _context.rentalContracts
                     .Include(h => h.habitacao) 
                     .FirstOrDefaultAsync(r => r.Id == id);


                    rc.DeliveryStatusId = delivery.Id;
                    rc.isConfirmed = true;
                    rc.habitacao.available = false;

                    _context.Update(rc);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalContractExists(confirmRentalContracts.rentalContract.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(confirmRentalContracts.rentalContract);
        }



        [Authorize(Roles = "Employer, Manager")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.rentalContracts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.rentalContracts'  is null.");
            }
            var rentalContract = await _context.rentalContracts.FindAsync(id);
            if (rentalContract != null)
            {
                rentalContract.habitacao = null;
                rentalContract.deliveryStatus = null;
                rentalContract.receiveStatus = null;
                rentalContract.user = null;

                _context.rentalContracts.Remove(rentalContract);
                ViewBag.SuccessMessage = "Categoria editada com sucesso";
            }

            await _context.SaveChangesAsync();

            if (User.IsInRole("Employer"))
            {
                var appUserId = _userManager.GetUserId(User);
                var employee = _context.employers.Where(uid => uid.user.Id == appUserId).FirstOrDefault();
                var _locador = _context.locador.FirstOrDefault(l => l.Id == employee.LocadorId);
                var applicationDbContext = _context.rentalContracts.Include(r => r.habitacao).Include(u => u.user).Where(l => l.habitacao.locador == _locador);
                return View("Index",await applicationDbContext.ToListAsync());

            }
            else if (User.IsInRole("Manager"))
            {
                var appUserId = _userManager.GetUserId(User);
                var manager = _context.managers.Where(uid => uid.user.Id == appUserId).FirstOrDefault();
                var _locador = _context.locador.FirstOrDefault(l => l.Id == manager.LocadorId);
                var applicationDbContext = _context.rentalContracts.Include(r => r.habitacao).Include(u => u.user).Where(l => l.habitacao.locador == _locador);
                return View("Index", await applicationDbContext.ToListAsync());
            }
            return View();
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
