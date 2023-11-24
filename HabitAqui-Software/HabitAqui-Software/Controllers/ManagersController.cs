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
using HabitAqui_Software.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace HabitAqui_Software.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ManagersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: Managers
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentManager = await _context.managers
                                               .FirstOrDefaultAsync(m => m.userId == currentUser.Id);

            var locadorId = currentManager.LocadorId;

            var usersWithRoles = new List<UserWithRolesViewModel>();
            var allUsers = _context.Users.ToList();

            foreach (var user in _context.Users.ToList())
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                if (userRoles.Contains("Employer") || userRoles.Contains("Manager"))
                {
                    var isSameLocadorId = _context.employers.Any(e => e.userId == user.Id && e.LocadorId == locadorId) ||
                                          _context.managers.Any(m => m.userId == user.Id && m.LocadorId == locadorId);

                    if (isSameLocadorId)
                    {
                        usersWithRoles.Add(new UserWithRolesViewModel
                        {
                            User = user,
                            Roles = userRoles.ToList()
                        });
                    }
                }
            }
            return View(usersWithRoles); // Incluir todos os usuários, incluindo o usuário atual
        }
        // GET: Managers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.managers == null)
            {
                return NotFound();
            }
            var manager = await _context.managers
                .Include(m => m.locador)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manager == null)
            {
                return NotFound();
            }
            return View(manager);
        }
        // GET: Managers/Create
        public IActionResult Create()
        {
            ViewData["LocadorId"] = new SelectList(_context.locador, "Id", "Id");
            return View();
        }
        // POST: Managers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LocadorId")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                _context.Add(manager);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocadorId"] = new SelectList(_context.locador, "Id", "Id", manager.LocadorId);
            return View(manager);
        }
        // GET: Managers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.managers == null)
            {
                return NotFound();
            }
            var manager = await _context.managers.FindAsync(id);
            if (manager == null)
            {
                return NotFound();
            }
            ViewData["LocadorId"] = new SelectList(_context.locador, "Id", "Id", manager.LocadorId);
            return View(manager);
        }
        // POST: Managers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LocadorId")] Manager manager)
        {
            if (id != manager.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManagerExists(manager.Id))
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
            ViewData["LocadorId"] = new SelectList(_context.locador, "Id", "Id", manager.LocadorId);
            return View(manager);
        }
        // GET: Managers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.managers == null)
            {
                return NotFound();
            }
            var manager = await _context.managers
                .Include(m => m.locador)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manager == null)
            {
                return NotFound();
            }
            return View(manager);
        }
        // POST: Managers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.managers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.managers'  is null.");
            }
            var manager = await _context.managers.FindAsync(id);
            if (manager != null)
            {
                _context.managers.Remove(manager);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ManagerExists(int id)
        {
            return (_context.managers?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        public async Task<IActionResult> CreateEmployeeAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var manager = await _context.managers.FirstOrDefaultAsync(m => m.user.Id == currentUser.Id);
            var locadorId = manager.LocadorId;
            var viewModel = new CreateEmployeeViewModel
            {
                LocadorId = locadorId
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    firstName = model.firstName,
                    lastName = model.lastName,
                    nif = model.nif,
                    available = true,
                    EmailConfirmed = true
                };
                var createUserResult = await _userManager.CreateAsync(user, model.Password);
                if (createUserResult.Succeeded)
                {

                    await _userManager.AddToRoleAsync(user, model.Role);
                    // Verificar o papel e criar a entidade apropriada
                    if (model.Role == "Employer")
                    {
                        var employer = new Employer
                        {
                            userId = user.Id,    // Associa o ID do usuário recém-criado
                            LocadorId = model.LocadorId  // Usa o LocadorId do ViewModel
                        };
                        _context.employers.Add(employer);  // Adiciona o novo registro à tabela Employer
                    }
                    else if (model.Role == "Manager")
                    {
                        var manager = new Manager
                        {
                            userId = user.Id,    // Associa o ID do usuário recém-criado
                            LocadorId = model.LocadorId  // Usa o LocadorId do ViewModel
                        };
                        _context.managers.Add(manager);  // Adiciona o novo registro à tabela Manager
                    }
                    await _context.SaveChangesAsync(); // Salva as alterações no banco de dados
                    return RedirectToAction("Index"); // Redirecionar para a página desejada
                }
                foreach (var error in createUserResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            // Se o modelo não for válido ou a criação falhar, recarregue a view
            return View(model);
        }
    }
}