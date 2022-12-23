using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrabalhoPratico.Data;
using TrabalhoPratico.Models;

namespace TrabalhoPratico.Controllers
{
    public class ClassificacoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClassificacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Classificacoes/Create
        public IActionResult Create(int? idReserva)
        {
            ViewData["EmpresaId"] = new SelectList(_context.Empresa, "Id", "Id");
            ViewData["ReservaId"] = new SelectList(_context.Reserva, "Id", "Id");
            return View();
        }

        // POST: Classificacoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmpresaId,ReservaId,ClassificacaoReserva")] Classificacao classificacao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classificacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpresaId"] = new SelectList(_context.Empresa, "Id", "Id", classificacao.EmpresaId);
            ViewData["ReservaId"] = new SelectList(_context.Reserva, "Id", "Id", classificacao.ReservaId);
            return View(classificacao);
        }

        // GET: Classificacoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Classificacao == null)
            {
                return NotFound();
            }

            var classificacao = await _context.Classificacao.FindAsync(id);
            if (classificacao == null)
            {
                return NotFound();
            }
            ViewData["EmpresaId"] = new SelectList(_context.Empresa, "Id", "Id", classificacao.EmpresaId);
            ViewData["ReservaId"] = new SelectList(_context.Reserva, "Id", "Id", classificacao.ReservaId);
            return View(classificacao);
        }
    }
}
