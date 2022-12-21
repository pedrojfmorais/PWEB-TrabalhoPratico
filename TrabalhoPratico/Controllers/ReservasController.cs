using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
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
        [Authorize(Roles = "Funcionario,Gestor")]
        public async Task<IActionResult> Index([Bind("TextoAPesquisar,Ordem,CategoriaId,DataLevantamento,DataEntrega")] PesquisaReservasGestorFuncionarioViewModel pesquisaReservas)
        {
            var user = await _userManager.GetUserAsync(User);

            IQueryable<Reserva> task = _context.Reserva
                .Include(r => r.Cliente)
                .Include(r => r.Veiculo)
                .Include(r => r.Veiculo.Categoria)
                .Include(r => r.ReservaEstadoVeiculoLevantamento)
                .Include(r => r.ReservaEstadoVeiculoEntrega)
                .Include(r => r.Veiculo.Empresa)
                .Where(r => r.Veiculo.EmpresaId == user.EmpresaId);


            if (pesquisaReservas.DataLevantamento == DateTime.Parse("01/01/0001 00:00:00"))
            {
                pesquisaReservas.DataLevantamento = DateTime.Now;
            }
            if (pesquisaReservas.DataEntrega == DateTime.Parse("01/01/0001 00:00:00"))
            {
                pesquisaReservas.DataEntrega = DateTime.Now;
            }

            if (string.IsNullOrWhiteSpace(pesquisaReservas.TextoAPesquisar))
            {
                task = task.OrderByDescending(r => r.Cliente.UserName).ThenByDescending(r => r.Veiculo.Matricula).ThenByDescending(r => r.Veiculo.Categoria.Nome);
            }
            else
            {
                task = task
                    .Where(r => (
                        r.Veiculo.Marca.Contains(pesquisaReservas.TextoAPesquisar)
                        || r.Veiculo.Modelo.Contains(pesquisaReservas.TextoAPesquisar)
                        || r.Veiculo.Matricula.Contains(pesquisaReservas.TextoAPesquisar)
                        || r.Cliente.UserName.Contains(pesquisaReservas.TextoAPesquisar)
                    )
                    && r.Veiculo.CategoriaId == pesquisaReservas.CategoriaId
                    && DateTime.Compare(r.DataLevantamento, pesquisaReservas.DataLevantamento) > 0
                    && DateTime.Compare(r.DataEntrega, pesquisaReservas.DataEntrega) < 0
                    );
            }

            if (pesquisaReservas.Ordem != null)
            {
                if (pesquisaReservas.Ordem.Equals("desc"))
                {
                    task = task.OrderByDescending(r => r.Cliente.UserName).ThenByDescending(r => r.Veiculo.Matricula).ThenByDescending(r => r.Veiculo.Categoria.Nome);
                }
                else if (pesquisaReservas.Ordem.Equals("asc"))
                {
                    task = task.OrderBy(r => r.Cliente.UserName).ThenBy(r => r.Veiculo.Matricula).ThenBy(r => r.Veiculo.Categoria.Nome);
                }
            }

            ViewBag.CategoriaId = new SelectList(_context.CategoriaVeiculo.ToList(), "Id", "Nome");

            pesquisaReservas.ListaDeReservas = await task.ToListAsync();

            if (pesquisaReservas.DataEntrega < pesquisaReservas.DataLevantamento)
            {
                ViewBag.error = "Datas de levantamento e entrega incorretas";
            }

            return View(pesquisaReservas);
        }

        // GET: Confirmar
        [Authorize(Roles = "Funcionario,Gestor")]
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
        [Authorize(Roles = "Funcionario,Gestor")]
        public async Task<IActionResult> LevantamentoAsync(int idReserva)
        {
            if (idReserva == null || _context.Reserva == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reserva.Where(r => r.Id == idReserva).Include(r => r.Veiculo).Include(r => r.Cliente).FirstOrDefaultAsync();
            if (reserva == null)
            {
                return NotFound();
            }

            ReservaEstadoVeiculoLevantamento reservaLevantamento = new ReservaEstadoVeiculoLevantamento();

            reservaLevantamento.Quilometros = reserva.Veiculo.Quilometros;

            reservaLevantamento.ReservaId = idReserva;
            reservaLevantamento.Reserva = reserva;

            return View(reservaLevantamento);
        }

        // POST: ReservaEstadoVeiculoes/Levantamento
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Funcionario,Gestor")]
        public async Task<IActionResult> Levantamento(
            [Bind("Id,Quilometros,DanosVeiculo,Observacoes,ReservaId,FuncionarioId")] ReservaEstadoVeiculoLevantamento reservaEstadoVeiculoLevantamento
            )
        {
            reservaEstadoVeiculoLevantamento.FuncionarioId = (await _userManager.GetUserAsync(User)).Id;
            reservaEstadoVeiculoLevantamento.Reserva = await _context.Reserva.Where(r => r.Id == reservaEstadoVeiculoLevantamento.ReservaId).Include(r => r.Veiculo).Include(r => r.Cliente).FirstAsync();
            
            ModelState.Remove("Funcionario");
            ModelState.Remove("Reserva");

            if (ModelState.IsValid)
            {
                reservaEstadoVeiculoLevantamento.Reserva.ReservaEstadoVeiculoLevantamentoId = reservaEstadoVeiculoLevantamento.Id;
                _context.Add(reservaEstadoVeiculoLevantamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(reservaEstadoVeiculoLevantamento);
        }

        // GET: ReservaEstadoVeiculoes/Entrega
        [Authorize(Roles = "Funcionario,Gestor")]
        public async Task<IActionResult> Entrega(int idReserva)
        {
            if (idReserva == null || _context.Reserva == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reserva.Where(r => r.Id == idReserva).Include(r => r.Veiculo).Include(r => r.Cliente).FirstOrDefaultAsync();
            if (reserva == null)
            {
                return NotFound();
            }

            ReservaEstadoVeiculoEntrega reservaEntrega = new ReservaEstadoVeiculoEntrega();

            reservaEntrega.Quilometros = reserva.Veiculo.Quilometros;

            reservaEntrega.ReservaId = idReserva;
            reservaEntrega.Reserva = reserva;

            return View(reservaEntrega);
        }

        // POST: ReservaEstadoVeiculoes/Entrega
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Funcionario,Gestor")]
        public async Task<IActionResult> Entrega(
            [Bind("Id,Quilometros,DanosVeiculo,Observacoes,ReservaId,FuncionarioId")] ReservaEstadoVeiculoEntrega reservaEntrega,
            [FromForm] List<IFormFile> provas
            )
        {

            reservaEntrega.FuncionarioId = (await _userManager.GetUserAsync(User)).Id;
            reservaEntrega.Reserva = await _context.Reserva.Where(r => r.Id == reservaEntrega.ReservaId).Include(r => r.Veiculo).Include(r => r.Cliente).FirstAsync();

            if (reservaEntrega.DanosVeiculo && provas.Count == 0)
            {
                ViewBag.error = "Insira as provas do dano ao veiculo";
                return View(reservaEntrega);
            }

            if (reservaEntrega.Reserva.Veiculo.Quilometros > reservaEntrega.Quilometros)
            {
                ViewBag.error = "Quilometros invalidos (os km baixaram?)";
                return View(reservaEntrega);
            }

            ModelState.Remove("Funcionario");
            ModelState.Remove("Reserva");

            if (ModelState.IsValid)
            {
                reservaEntrega.Reserva.ReservaEstadoVeiculoLevantamentoId = reservaEntrega.Id;
                _context.Add(reservaEntrega);

                var veiculo = await _context.Veiculo.Where(v => v.Id == reservaEntrega.Reserva.Veiculo.Id).FirstAsync();
                veiculo.Quilometros = reservaEntrega.Quilometros;

                _context.Update(veiculo);

                await _context.SaveChangesAsync();

                if (reservaEntrega.DanosVeiculo)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/provasDanosVeiculos/");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    // Dir relativo aos ficheiros do curso
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/provasDanosVeiculos/" + reservaEntrega.Id.ToString());
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    foreach (var formFile in provas)
                    {
                        if (formFile.Length > 0)
                        {
                            var filePath = Path.Combine(path, Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName));
                            while (System.IO.File.Exists(filePath))
                            {
                                filePath = Path.Combine(path, Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName));
                            }
                            using (var stream = System.IO.File.Create(filePath))
                            {
                                await formFile.CopyToAsync(stream);
                            }
                        }
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(reservaEntrega);
        }


        // Cliente
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
