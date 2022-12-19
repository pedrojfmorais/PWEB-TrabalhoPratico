using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrabalhoPratico.Data;
using TrabalhoPratico.Models;

namespace TrabalhoPratico.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /*
         
        Funcionario, Gestor
         
         */

        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reserva
                .Include(r => r.Cliente)
                .Include(r => r.Veiculo)
                .Include(r => r.Veiculo.Categoria)
                .Include(r => r.ReservaEstadoVeiculoLevantamento)
                .Include(r => r.ReservaEstadoVeiculoEntrega);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Confirmar
        public async Task<IActionResult> Confirmar(int? id)
        {
            if (id == null || _context.Reserva == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reserva
                .FirstOrDefaultAsync(r => r.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            reserva.Confirmada = true;
            _context.Update(reserva);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        
        // GET: ReservaEstadoVeiculoes/Levantamento
        public async Task<IActionResult> LevantamentoAsync(int? id)
        {
            if (id == null || _context.Reserva == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reserva.Where(r => r.Id == id).Include(r => r.Veiculo).Include(r => r.Cliente).FirstOrDefaultAsync();
            if (reserva == null)
            {
                return NotFound();
            }

            ReservaEstadoVeiculoLevantamento reservaLevantamento = new ReservaEstadoVeiculoLevantamento();

            reservaLevantamento.Quilometros = reserva.Veiculo.Quilometros;

            reservaLevantamento.ReservaId = id;
            reservaLevantamento.Reserva = reserva;

            return View(reservaLevantamento);
        }

        // POST: ReservaEstadoVeiculoes/Levantamento
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Levantamento(
            [Bind("Id,Quilometros,DanosVeiculo,Observacoes")] ReservaEstadoVeiculoLevantamento reservaEstadoVeiculoLevantamento,
            int? id)
        {
            reservaEstadoVeiculoLevantamento.FuncionarioId = (await _userManager.GetUserAsync(User)).Id;
            reservaEstadoVeiculoLevantamento.ReservaId = id;
            reservaEstadoVeiculoLevantamento.Reserva = await _context.Reserva.Where(r => r.Id == id).Include(r => r.Veiculo).Include(r => r.Cliente).FirstAsync();
            
            ModelState.Remove("Funcionario");
            ModelState.Remove("Reserva");

            if (ModelState.IsValid)
            {
                reservaEstadoVeiculoLevantamento.Reserva.ReservaEstadoVeiculoLevantamentoId = reservaEstadoVeiculoLevantamento.Id;
                _context.Add(reservaEstadoVeiculoLevantamento);
                _context.Update(reservaEstadoVeiculoLevantamento.Reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(reservaEstadoVeiculoLevantamento);
        }
        
        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reserva == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reserva
                .Include(r => r.Cliente)
                .Include(r => r.Veiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // GET: Reservas/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["VeiculoId"] = new SelectList(_context.Veiculo, "Id", "Id");
            return View();
        }

        // POST: Reservas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DataLevantamento,DataEntrega,Confirmada,VeiculoId,ClienteId,ReservaEstadoVeiculoLevantamentoId,ReservaEstadoVeiculoEntregaId")] Reserva reserva)
        {
            ModelState.Remove("Cliente");
            ModelState.Remove("Veiculo");
            if (ModelState.IsValid)
            {
                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Users, "Id", "Id", reserva.ClienteId);
            ViewData["VeiculoId"] = new SelectList(_context.Veiculo, "Id", "Id", reserva.VeiculoId);
            return View(reserva);
        }

        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reserva == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reserva.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Users, "Id", "Id", reserva.ClienteId);
            ViewData["VeiculoId"] = new SelectList(_context.Veiculo, "Id", "Id", reserva.VeiculoId);
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DataLevantamento,DataEntrega,Confirmada,VeiculoId,ClienteId,ReservaEstadoVeiculoLevantamentoId,ReservaEstadoVeiculoEntregaId")] Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Users, "Id", "Id", reserva.ClienteId);
            ViewData["VeiculoId"] = new SelectList(_context.Veiculo, "Id", "Id", reserva.VeiculoId);
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reserva == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reserva
                .Include(r => r.Cliente)
                .Include(r => r.Veiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reserva == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reserva'  is null.");
            }
            var reserva = await _context.Reserva.FindAsync(id);
            if (reserva != null)
            {
                _context.Reserva.Remove(reserva);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservaExists(int id)
        {
          return _context.Reserva.Any(e => e.Id == id);
        }
    }
}
