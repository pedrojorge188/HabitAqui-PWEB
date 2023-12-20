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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace HabitAqui_Software.Controllers
{

    [Authorize(Roles = "Client, Employer, Manager")]
    public class RentalContractsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public RentalContractsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
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
                var employee = _context.employers.FirstOrDefault(uid => uid.user.Id == appUserId);
                var _locador = _context.locador.FirstOrDefault(l => l.Id == employee.LocadorId);

                var contractsByUser = _context.rentalContracts
                    .Where(rc => rc.isConfirmed && rc.habitacao.LocadorId == _locador.Id)
                    .GroupBy(rc => rc.user.UserName)
                    .Select(group => new { userName = group.Key, Count = group.Count() })
                    .ToList();

                ViewBag.ContractsData = contractsByUser;

                var applicationDbContext = _context.rentalContracts
                    .Include(r => r.habitacao)
                    .Include(u => u.user)
                    .Where(l => l.habitacao.locador == _locador);

                return View(await applicationDbContext.ToListAsync());
            }
            else if (User.IsInRole("Manager"))
            {
                var appUserId = _userManager.GetUserId(User);
                var manager = _context.managers.Where(uid => uid.user.Id == appUserId).FirstOrDefault();
                var _locador = _context.locador.FirstOrDefault(l => l.Id == manager.LocadorId);
                var contractsByUser = _context.rentalContracts
                   .Where(rc => rc.isConfirmed && rc.habitacao.LocadorId == _locador.Id)
                   .GroupBy(rc => rc.user.UserName)
                   .Select(group => new { userName = group.Key, Count = group.Count() })
                   .ToList();

                ViewBag.ContractsData = contractsByUser;
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

            var ret = await _context.rentalContracts
              .Include(rc => rc.habitacao)        
              .Include(rc => rc.deliveryStatus)    
              .Include(rc => rc.receiveStatus)  
              .Include(rc => rc.user) 
              .FirstOrDefaultAsync(l => l.Id == id);

            if (ret == null)
            {
                return NotFound();
            }

            return View(ret);
        }


    // GET: RentalContracts/Create
    [Authorize(Roles = "Employer, Manager")]
        public IActionResult Create()
        {

            var habitationsId = _context.rentalContracts.Where(rc => rc.receiveStatus == null).Select(rc => rc.HabitacaoId).ToList();
            if (habitationsId.Count == 0)
            {
                habitationsId = _context.rentalContracts.Select(rc => rc.HabitacaoId).ToList();
            }

            if (User.IsInRole("Employer"))
            {
                var appUserId = _userManager.GetUserId(User);
                var employee = _context.employers.Where(uid => uid.user.Id == appUserId).FirstOrDefault();
                var _locador = _context.locador.FirstOrDefault(l => l.Id == employee.LocadorId);
                var habitationList = _context.habitacaos
                .Where(av => av.available == true && av.LocadorId == _locador.Id)
                .ToList();
                var users = _userManager.GetUsersInRoleAsync("Client").Result.ToList();
                ViewData["HabitacaoId"] = new SelectList(habitationList, "Id", "location");
                ViewData["userId"] = new SelectList(users, "Id", "firstName");
            }
            else if (User.IsInRole("Manager"))
            {
                var appUserId = _userManager.GetUserId(User);
                var manager = _context.managers.Where(uid => uid.user.Id == appUserId).FirstOrDefault();
                var _locador = _context.locador.FirstOrDefault(l => l.Id == manager.LocadorId);
                var habitationList = _context.habitacaos
               .Where(av => av.available == true && av.LocadorId == _locador.Id)
               .ToList();
                var users = _userManager.GetUsersInRoleAsync("Client").Result.ToList();
                ViewData["HabitacaoId"] = new SelectList(habitationList, "Id", "location");
                ViewData["userId"] = new SelectList(users, "Id", "firstName");
            }

            return View();
        }

        //&& !habitationsId.Contains(av.Id)

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employer, Manager")]
        public async Task<IActionResult> Create([Bind("Id,startDate,endDate,isConfirmed,HabitacaoId,DeliveryStatusId,ReceiveStatusId,userId")] RentalContract rentalContract)
        {
            // Preencher SelectList para ViewData
            void PreencherSelectList()
            {
                var habitationList = _context.habitacaos.Where(av => av.available == true).ToList();
                var users = _userManager.GetUsersInRoleAsync("Client").Result.ToList();
                ViewData["HabitacaoId"] = new SelectList(habitationList, "Id", "location");
                ViewData["userId"] = new SelectList(users, "Id", "firstName");
            }

            if (ModelState.IsValid)
            {
                ViewBag.SuccessMessage = "Contrato criado com sucesso";
                rentalContract.avaliacao = 0;
                rentalContract.isConfirmed = false;

                _context.Add(rentalContract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PreencherSelectList();
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employer, Manager")]
        public async Task<IActionResult> Confirm(int id, ConfirmRentalContracts confirmRentalContracts)
        {
            var rentalContract = await _context.rentalContracts
                .Include(h => h.habitacao)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rentalContract == null)
            {
                return NotFound();
            }

            // Verifica se o diretório para uploads de imagens existe, se não, cria
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img_upload");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = null;
            List<string> savedFiles = new List<string>();

            // Processa o arquivo de imagem recebido
            if (confirmRentalContracts.DamageImages != null && confirmRentalContracts.DamageImages.Count > 0)
            {

                foreach (var imageFile in confirmRentalContracts.DamageImages)
                {
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var image = Image.Load(imageFile.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(100, 100));
                        await image.SaveAsync(filePath); // Salva cada imagem redimensionada
                    }

                    savedFiles.Add(uniqueFileName); // Adiciona o nome do arquivo salvo à lista
                }
            }

            string imagePaths = string.Join(";", savedFiles);

            string equipmentList = confirmRentalContracts.equipments != null ? string.Join(";", confirmRentalContracts.equipments) : string.Empty;

            DeliveryStatus delivery = new DeliveryStatus
            {
                hasDamage = confirmRentalContracts.hasDamage,
                hasEquipments = confirmRentalContracts.hasEquipments,
                observation = confirmRentalContracts.observation,
                RentalContractId = id,
                ImagePaths = imagePaths,
                damageDescription = confirmRentalContracts.damageDescription,
                EquipmentList = equipmentList
            };

            _context.Add(delivery);
            await _context.SaveChangesAsync();

            rentalContract.DeliveryStatusId = delivery.Id;
            rentalContract.isConfirmed = true;
            rentalContract.habitacao.available = false;

            _context.Update(rentalContract);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
                
        }


        // GET: Receber habitação
        [Authorize(Roles = "Employer, Manager")]
        public async Task<IActionResult> Receive(int? id)
        {
            var rentalContract = await _context.rentalContracts
                .Include(rc => rc.deliveryStatus) // Garante que o deliveryStatus seja carregado
                .FirstOrDefaultAsync(rc => rc.Id == id);

            if (id == null || _context.rentalContracts == null)
            {
                return NotFound();
            }

            var viewModel = new ReceiveRentalContract
            {
                rentalContract = await _context.rentalContracts.FindAsync(id)
            };

            if (viewModel.rentalContract == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: Confirmar recebimento da habitação
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Receive(int id, ReceiveRentalContract viewModel)
        {
            if (id != viewModel.rentalContract.Id)
            {
                return NotFound();
            }

            // Verifica se o diretório para uploads de imagens existe, se não, cria
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img_upload");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = null;
            List<string> savedFiles = new List<string>();

            // Processa o arquivo de imagem recebido
            if (viewModel.DamageImages != null && viewModel.DamageImages.Count > 0)
            {

                foreach (var imageFile in viewModel.DamageImages)
                {
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var image = Image.Load(imageFile.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(100, 100));
                        await image.SaveAsync(filePath); // Salva cada imagem redimensionada
                    }

                    savedFiles.Add(uniqueFileName); // Adiciona o nome do arquivo salvo à lista
                }
            }

            string imagePaths = string.Join(";", savedFiles);

            // Verifica se a lista de equipamentos não é nula antes de juntar
            string equipmentList = viewModel.equipments != null ? string.Join(";", viewModel.equipments) : string.Empty;

            ReceiveStatus receiveStatus = new ReceiveStatus
            {
                hasDamage = viewModel.hasDamage,
                hasEquipments = viewModel.hasEquipments,
                damageDescription = viewModel.damageDescription,
                rentalContractId = viewModel.rentalContract.Id,
                // anexar fotos de danos, se aplicável
                observation = viewModel.observation,
                ImagePaths = imagePaths,
                EquipmentList = equipmentList
            };

            _context.Add(receiveStatus);
            await _context.SaveChangesAsync();

   
            RentalContract rentalContract = await _context.rentalContracts
                .Include(h => h.habitacao)
                .FirstOrDefaultAsync(r => r.Id == id);

            rentalContract.ReceiveStatusId = receiveStatus.Id;
            rentalContract.habitacao.available = true;

            _context.Update(rentalContract);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

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
                ViewBag.SuccessMessage = "Arrendamento recusado com sucesso";
            }

            await _context.SaveChangesAsync();

            if (User.IsInRole("Employer"))
            {
                var appUserId = _userManager.GetUserId(User);
                var employee = _context.employers.Where(uid => uid.user.Id == appUserId).FirstOrDefault();
                var _locador = _context.locador.FirstOrDefault(l => l.Id == employee.LocadorId);
                var applicationDbContext = _context.rentalContracts.Include(r => r.habitacao).Include(u => u.user).Include(r => r.receiveStatus).Where(l => l.habitacao.locador == _locador);
                return View("Index",await applicationDbContext.ToListAsync());

            }
            else if (User.IsInRole("Manager"))
            {
                var appUserId = _userManager.GetUserId(User);
                var manager = _context.managers.Where(uid => uid.user.Id == appUserId).FirstOrDefault();
                var _locador = _context.locador.FirstOrDefault(l => l.Id == manager.LocadorId);
                var applicationDbContext = _context.rentalContracts.Include(r => r.habitacao).Include(u => u.user).Include(r => r.receiveStatus).Where(l => l.habitacao.locador == _locador);
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
                                                                                                              ren.isConfirmed == true && ren.receiveStatus != null);

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

                RentalContract rc = await _context.rentalContracts
            .Include(r => r.habitacao) // Inclui a habitação relacionada
            .FirstOrDefaultAsync(r => r.Id == id);

            if (rc == null)
            {
                return NotFound();
            }

            rc.avaliacao = rentalContract.avaliacao;
            _context.Update(rc);
            await _context.SaveChangesAsync();

            // Calcular a média de avaliações para a habitação
            var habitacaoId = rc.HabitacaoId;
            if (habitacaoId.HasValue)
            {
                var habitacao = await _context.habitacaos
                    .Include(h => h.rentalContracts)
                    .FirstOrDefaultAsync(h => h.Id == habitacaoId.Value);

                if (habitacao != null && habitacao.rentalContracts.Any())
                {
                    // Calcula a média das avaliações, excluindo zeros
                    double averageGrade = habitacao.rentalContracts
                        .Where(c => c.avaliacao.HasValue && c.avaliacao.Value >= 1 && c.avaliacao.Value <= 5)
                        .Average(c => c.avaliacao.Value);

                    habitacao.grade = (float)Math.Round(averageGrade, 2);
                    _context.Update(habitacao);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(History));
        }
    }
}
