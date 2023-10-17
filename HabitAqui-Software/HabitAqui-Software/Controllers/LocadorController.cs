﻿using System;
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
    public class LocadorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocadorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Locador
        public async Task<IActionResult> Index()
        {
            ViewData["Enrollments"] = _context.enrollments.ToList();
            return _context.locador != null ?
                        View(await _context.locador.Include(h => h.enrollmentState).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.locador'  is null.");
        }

        // GET: Locador/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.locador == null)
            {
                return NotFound();
            }

            var locador = await _context.locador
                .FirstOrDefaultAsync(m => m.Id == id);
            if (locador == null)
            {
                return NotFound();
            }

            return View(locador);
        }

        // GET: Locador/Create
        public IActionResult Create()
        {
   
            return View();
        }

        // POST: Locador/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,company,address,email")] Locador locador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(locador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(locador);
        }

        // GET: Locador/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
           
            if (id == null || _context.locador == null)
            {
                return NotFound();
            }

            var locador = await _context.locador.FindAsync(id);

            ViewData["Enrollments"] = _context.enrollments.ToList();

            try
            {
                ViewBag.CurrentEnroll = locador.enrollmentState.name;

            }catch(Exception ex)
            {
                ViewBag.CurrentEnroll = "Não tem!";
            }

            if (locador == null)
            {
                return NotFound();
            }
            return View(locador);
        }

        // POST: Locador/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,company,address,email,enrollmentState")] Locador locador)
        {
            if (id != locador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(locador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocadorExists(locador.Id))
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
            return View(locador);
        }

        // GET: Locador/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.locador == null)
            {
                return NotFound();
            }

            var locador = await _context.locador
                .FirstOrDefaultAsync(m => m.Id == id);
            if (locador == null)
            {
                return NotFound();
            }

            return View(locador);
        }

        // POST: Locador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.locador == null)
            {
                ViewBag.ErrorMessage = "ERRO: Locador associado a uma habitação";
                return Problem("Entity set 'ApplicationDbContext.locador'  is null.");
            }

            var isForeignKeyInUse = _context.habitacaos.Any(h => h.locador.Id == id);

            if (isForeignKeyInUse)
            {
                ViewBag.ErrorMessage = "ERRO: Locador associado a uma habitação";
                return View(await _context.locador.FindAsync(id));
            }

            var locador = await _context.locador.FindAsync(id);

            if (locador != null)
            {
                ViewBag.SuccessMessage = "Locador removido com sucesso";
                _context.locador.Remove(locador);
            }



            await _context.SaveChangesAsync();
            return View("Index", await _context.locador.Include(it=>it.enrollmentState).ToListAsync());
        }

        private bool LocadorExists(int id)
        {
            return (_context.locador?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
