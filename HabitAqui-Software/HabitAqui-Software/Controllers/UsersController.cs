using HabitAqui_Software.Data;
using HabitAqui_Software.Models;
using HabitAqui_Software.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui_Software.Controllers
{
    //[Authorize(Roles = "Admin, Manager")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var usersWithRoles = new List<UserWithRolesViewModel>();

            var currentUser = await _userManager.GetUserAsync(User);

            foreach (var user in _context.Users.Where(u => u.Id != currentUser.Id).ToList())
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                usersWithRoles.Add(new UserWithRolesViewModel
                {
                    User = user,
                    Roles = userRoles.ToList()
                });
            }

            return View(usersWithRoles);
        }

        public async Task<ActionResult> Search(string? name)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var usersWithRoles = new List<UserWithRolesViewModel>();

            if (name != null)
            {
                var filteredUsers = _context.Users
              .Where(u => (u.firstName + " " + u.lastName).Contains(name) && u.Id != currentUser.Id)
              .ToList();

                foreach (var user in filteredUsers)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    usersWithRoles.Add(new UserWithRolesViewModel
                    {
                        User = user,
                        Roles = userRoles.ToList()
                    });
                       
                 }
                
                return View("Index", usersWithRoles);
            }

            foreach (var user in _context.Users.Where(u => u.Id != currentUser.Id).ToList())
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                usersWithRoles.Add(new UserWithRolesViewModel
                {
                    User = user,
                    Roles = userRoles.ToList()
                });
            }

            return View("Index", usersWithRoles);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            var result = _userManager.IsInRoleAsync(user, "Admin");
            if (await result)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, [Bind("Id,firstName,lastName,bornDate,nif")] ApplicationUser userToUpdate)
        {
            if (id != userToUpdate.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.Users.FindAsync(id);

                    if (existingUser == null)
                        return NotFound();

                    // Atualizar apenas os campos específicos
                    existingUser.firstName = userToUpdate.firstName;
                    existingUser.lastName = userToUpdate.lastName;
                    existingUser.bornDate = userToUpdate.bornDate;
                    existingUser.nif = userToUpdate.nif;

                    _context.Update(existingUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    ViewBag.ErrorMessage = "Ocorreu um erro";
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(userToUpdate);
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Inative(string id)
        {
            TempData["UrlAnterior"] = Request.Headers["Referer"].ToString();
            //Console.WriteLine("URL de validação: " + Request.Headers["Referer"].ToString());

            if (ModelState.IsValid)
            {   
                await makeUserAvailableUnavailable(id);
                ViewBag.SuccessMessage = "Disponibilidade do utilizador alterada com sucesso!";
            }

            var urlAnterior = TempData["UrlAnterior"]?.ToString();

            if (!string.IsNullOrEmpty(urlAnterior))
            {
                return Redirect(urlAnterior);
            }

            return RedirectToAction("Index"); // Página inicial de fallback se não houver URL anterior
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> makeUserAvailableUnavailable(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var currentUser = await _context.Users.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (currentUser == null)
            {
                return NotFound();
            }
            if (currentUser.available == true)
            {

                currentUser.available = false;
                var task = _userManager.FindByEmailAsync(currentUser.Email);
                task.Wait();
                var user = task.Result;
                var lockUserTask = _userManager.SetLockoutEnabledAsync(user, true);
                lockUserTask.Wait();
                var lockDateTask = _userManager.SetLockoutEndDateAsync(user, DateTime.MaxValue);
                lockDateTask.Wait();
            }
            else
            {
                currentUser.available = true;
                var userTask = _userManager.FindByEmailAsync(currentUser.Email);
                userTask.Wait();
                var user = userTask.Result;
                var lockDisabledTask = _userManager.SetLockoutEnabledAsync(user, false);
                lockDisabledTask.Wait();
                var setLockoutEndDateTask = _userManager.SetLockoutEndDateAsync(user, DateTime.Now - TimeSpan.FromMinutes(1));
                setLockoutEndDateTask.Wait();
            }
            try
            {
                _context.Update(currentUser);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Delete(string id)
        {
            TempData["UrlAnterior"] = Request.Headers["Referer"].ToString();

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Impedir que o usuário atual se exclua
            var currentUser = await _userManager.GetUserAsync(User);
            if (user.Id == currentUser.Id)
            {
                // Retornar um erro ou redirecionar, conforme a lógica do seu aplicativo
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            Console.WriteLine("ID user: " + id);

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var urlAnterior = TempData["UrlAnterior"]?.ToString();

            // Impedir que o usuário atual se exclua
            var currentUser = await _userManager.GetUserAsync(User);
            if (user.Id == currentUser.Id)
            {
                if (!string.IsNullOrEmpty(urlAnterior))
                {
                    return Redirect(urlAnterior);
                }

                return RedirectToAction("Index"); // Página inicial de fallback se não houver URL anterior
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            if (!string.IsNullOrEmpty(urlAnterior))
            {
                return Redirect(urlAnterior);
            }

            return RedirectToAction("Index"); // Página inicial de fallback se não houver URL anterior
        }

        public IActionResult VoltarParaAnterior()
        {
            var urlAnterior = TempData["UrlAnterior"]?.ToString();

            Console.WriteLine("URL de validação: " + urlAnterior);

            if (!string.IsNullOrEmpty(urlAnterior))
            {
                return Redirect(urlAnterior);
            }
            return RedirectToAction("Index"); // Página inicial de fallback se não houver URL anterior
        }

    }
}
