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
using TrabalhoPratico.Models.ViewModels;

namespace TrabalhoPratico.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class CategoriaVeiculosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriaVeiculosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CategoriaVeiculos
        public async Task<IActionResult> Index([Bind("TextoAPesquisar,Ordem")] PesquisaCategoriaViewModel pesquisaCategoria)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
                TempData.Remove("error");
            }

            IQueryable<CategoriaVeiculo> task;
            if (string.IsNullOrWhiteSpace(pesquisaCategoria.TextoAPesquisar))
            {
                task = _context.CategoriaVeiculo.Where(e => e.Nome.Contains("")).OrderByDescending(e => e.Nome);
            }
            else
            {
                task = _context.CategoriaVeiculo.Where(e => e.Nome.Contains(pesquisaCategoria.TextoAPesquisar));
            }
            if (pesquisaCategoria.Ordem != null)
            {
                if (pesquisaCategoria.Ordem.Equals("nomeDesc"))
                {
                    task = task.OrderByDescending(e => e.Nome);
                }
                else if (pesquisaCategoria.Ordem.Equals("nomeAsc"))
                {
                    task = task.OrderBy(e => e.Nome);
                }
            }

            pesquisaCategoria.ListaDeCategorias = await task.ToListAsync();

            return View(pesquisaCategoria);
        }

        // GET: CategoriaVeiculos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoriaVeiculos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] CategoriaVeiculo categoriaVeiculo)
        {

            ModelState.Remove("Veiculos");
            if (ModelState.IsValid)
            {
                _context.Add(categoriaVeiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoriaVeiculo);
        }

        // GET: CategoriaVeiculos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CategoriaVeiculo == null)
            {
                return NotFound();
            }

            var categoriaVeiculo = await _context.CategoriaVeiculo.FindAsync(id);
            if (categoriaVeiculo == null)
            {
                return NotFound();
            }
            return View(categoriaVeiculo);
        }

        // POST: CategoriaVeiculos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] CategoriaVeiculo categoriaVeiculo)
        {
            if (id != categoriaVeiculo.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Veiculos");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoriaVeiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaVeiculoExists(categoriaVeiculo.Id))
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
            return View(categoriaVeiculo);
        }

        // GET: CategoriaVeiculos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CategoriaVeiculo == null)
            {
                return NotFound();
            }

            var categoriaVeiculo = await _context.CategoriaVeiculo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoriaVeiculo == null)
            {
                return NotFound();
            }

            if (_context.Veiculo.Where(v => v.CategoriaId== id).Any())
            {
                TempData["error"] = "A categoria " + categoriaVeiculo.Nome + " ainda tem veiculos para aluguer.";
                return RedirectToAction(nameof(Index));
            }

            return View(categoriaVeiculo);
        }

        // POST: CategoriaVeiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CategoriaVeiculo == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CategoriaVeiculo'  is null.");
            }
            var categoriaVeiculo = await _context.CategoriaVeiculo.FindAsync(id);
            if (categoriaVeiculo != null)
            {
                _context.CategoriaVeiculo.Remove(categoriaVeiculo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaVeiculoExists(int id)
        {
          return _context.CategoriaVeiculo.Any(e => e.Id == id);
        }
    }
}
