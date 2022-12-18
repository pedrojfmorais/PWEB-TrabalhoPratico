using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrabalhoPratico.Data;
using TrabalhoPratico.Models;

namespace TrabalhoPratico.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class LocalizacaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocalizacaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Localizacao
        public async Task<IActionResult> Index()
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
                TempData.Remove("error");
            }

            return View(await _context.Localizacao.ToListAsync());
        }

        // GET: Localizacao/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Localizacao/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Localizacao localizacao)
        {
            ModelState.Remove("Veiculos");
            if (ModelState.IsValid)
            {
                _context.Add(localizacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(localizacao);
        }

        // GET: Localizacao/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Localizacao == null)
            {
                return NotFound();
            }

            var localizacao = await _context.Localizacao.FindAsync(id);
            if (localizacao == null)
            {
                return NotFound();
            }
            return View(localizacao);
        }

        // POST: Localizacao/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Localizacao localizacao)
        {
            if (id != localizacao.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Veiculos");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(localizacao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocalizacaoExists(localizacao.Id))
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
            return View(localizacao);
        }

        // GET: Localizacao/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Localizacao == null)
            {
                return NotFound();
            }

            var localizacao = await _context.Localizacao
                .FirstOrDefaultAsync(m => m.Id == id);
            if (localizacao == null)
            {
                return NotFound();
            }

            if (_context.Veiculo.Where(v => v.LocalizacaoId== id).Any())
            {
                TempData["error"] = "A localização " + localizacao.Nome + " ainda tem veiculos para aluguer.";
                return RedirectToAction(nameof(Index));
            }

            return View(localizacao);
        }

        // POST: Localizacao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Localizacao == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Localizacao'  is null.");
            }
            var localizacao = await _context.Localizacao.FindAsync(id);
            if (localizacao != null)
            {
                _context.Localizacao.Remove(localizacao);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocalizacaoExists(int id)
        {
          return _context.Localizacao.Any(e => e.Id == id);
        }
    }
}
