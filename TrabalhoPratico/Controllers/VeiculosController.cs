﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrabalhoPratico.Data;
using TrabalhoPratico.Models;
using TrabalhoPratico.Models.ViewModels;

namespace TrabalhoPratico.Controllers
{
    [Authorize(Roles = "Funcionario,Gestor")]
    public class VeiculosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VeiculosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Veiculos
        public async Task<IActionResult> Index(
            [Bind("TextoAPesquisar,Ordem,CategoriaId")] PesquisaFrotaVeiculosViewModel pesquisaVeiculos,
            string Disponivel)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
                TempData.Remove("error");
            }

            IQueryable<Veiculo> task = _context.Veiculo.Include(v => v.Categoria).Include(v => v.Empresa).Include(v => v.Localizacao);
            if (string.IsNullOrWhiteSpace(pesquisaVeiculos.TextoAPesquisar))
            {
                task = task.OrderByDescending(v => v.Marca).ThenByDescending(v => v.Modelo).ThenByDescending(v => v.Ano);
            }
            else
            {
                if (Disponivel != null && Disponivel.Equals("on"))
                {
                    pesquisaVeiculos.Disponivel = true;
                }

                task = task
                    .Where(e => (
                        e.Marca.Contains(pesquisaVeiculos.TextoAPesquisar)
                        || e.Modelo.Contains(pesquisaVeiculos.TextoAPesquisar)
                        || e.Ano.ToString().Contains(pesquisaVeiculos.TextoAPesquisar)
                    ) 
                    && e.Disponivel == pesquisaVeiculos.Disponivel
                    && e.CategoriaId == pesquisaVeiculos.CategoriaId);
            }

            if (pesquisaVeiculos.Ordem != null)
            {
                if (pesquisaVeiculos.Ordem.Equals("desc"))
                {
                    task = task.OrderByDescending(v => v.Marca).ThenByDescending(v => v.Modelo).ThenByDescending(v => v.Ano);
                }
                else if (pesquisaVeiculos.Ordem.Equals("asc"))
                {
                    task = task.OrderBy(v => v.Marca).ThenBy(v => v.Modelo).ThenBy(v => v.Ano);
                }
            }

            ViewBag.CategoriaId = new SelectList(_context.CategoriaVeiculo.ToList(), "Id", "Nome");

            pesquisaVeiculos.ListaDeVeiculos = await task.ToListAsync();

            return View(pesquisaVeiculos);
        }

        // GET: Veiculos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Veiculo == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculo
                .Include(v => v.Categoria)
                .Include(v => v.Empresa)
                .Include(v => v.Localizacao)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }

            return View(veiculo);
        }

        // GET: Ativar
        public async Task<IActionResult> Ativar(int? id)
        {
            if (id == null || _context.Veiculo == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }

            veiculo.Disponivel = true;
            _context.Update(veiculo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Desativar
        public async Task<IActionResult> Desativar(int? id)
        {
            if (id == null || _context.Veiculo == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }

            veiculo.Disponivel = false;
            _context.Update(veiculo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Veiculos/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.CategoriaVeiculo, "Id", "Nome");
            ViewBag.LocalizacaoId = new SelectList(_context.Localizacao, "Id", "Nome");
            return View();
        }

        // POST: Veiculos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Marca,Modelo,Ano,Matricula,Quilometros,Disponivel,PrecoDia,CategoriaId,LocalizacaoId")] Veiculo veiculo)
        {
            veiculo.EmpresaId = (int)(await _userManager.GetUserAsync(User)).EmpresaId;

            ModelState.Remove("Categoria");
            ModelState.Remove("Localizacao");
            ModelState.Remove("Empresa");
            if (ModelState.IsValid)
            {
                _context.Add(veiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoriaId = new SelectList(_context.CategoriaVeiculo, "Id", "Nome", veiculo.CategoriaId);
            ViewBag.LocalizacaoId = new SelectList(_context.Localizacao, "Id", "Nome", veiculo.LocalizacaoId);
            return View(veiculo);
        }

        // GET: Veiculos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Veiculo == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculo.FindAsync(id);
            if (veiculo == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.CategoriaVeiculo, "Id", "Nome", veiculo.CategoriaId);
            ViewData["LocalizacaoId"] = new SelectList(_context.Localizacao, "Id", "Nome", veiculo.LocalizacaoId);
            return View(veiculo);
        }

        // POST: Veiculos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Marca,Modelo,Ano,Matricula,Quilometros,Disponivel,PrecoDia,CategoriaId,LocalizacaoId")] Veiculo veiculo)
        {
            if (id != veiculo.Id)
            {
                return NotFound();
            }
            veiculo.EmpresaId = (int)(await _userManager.GetUserAsync(User)).EmpresaId;

            ModelState.Remove("Categoria");
            ModelState.Remove("Localizacao");
            ModelState.Remove("Empresa");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(veiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeiculoExists(veiculo.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.CategoriaVeiculo, "Id", "Nome", veiculo.CategoriaId);
            ViewData["LocalizacaoId"] = new SelectList(_context.Localizacao, "Id", "Nome", veiculo.LocalizacaoId);
            return View(veiculo);
        }

        // GET: Veiculos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Veiculo == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculo
                .Include(v => v.Categoria)
                .Include(v => v.Empresa)
                .Include(v => v.Localizacao)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }

            if (_context.Reserva.Where(r => r.VeiculoId == id).Any())
            {
                TempData["error"] = "O veiculo " + veiculo.Marca + " " + veiculo.Modelo + " (" + veiculo.Matricula + ") tem reservas associadas.";
                return RedirectToAction(nameof(Index));
            }

            return View(veiculo);
        }

        // POST: Veiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Veiculo == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Veiculo'  is null.");
            }
            var veiculo = await _context.Veiculo.FindAsync(id);
            if (veiculo != null)
            {
                _context.Veiculo.Remove(veiculo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VeiculoExists(int id)
        {
          return _context.Veiculo.Any(e => e.Id == id);
        }
    }
}