using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using TrabalhoPratico.Data;
using TrabalhoPratico.Models;
using TrabalhoPratico.Models.ViewModels;

namespace TrabalhoPratico.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class EmpresasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmpresasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        // GET: Empresas
        public async Task<IActionResult> Index(
            [Bind("TextoAPesquisar,SubscricaoAtiva,Ordem")] PesquisaEmpresaViewModel pesquisaEmpresa)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
                TempData.Remove("error");
            }

            IQueryable<Empresa> task = _context.Empresa.Include(e => e.Classificacoes);
            if (!string.IsNullOrWhiteSpace(pesquisaEmpresa.TextoAPesquisar))
            {
                task = task.Where(e => e.Nome.Contains(pesquisaEmpresa.TextoAPesquisar));
            }

            if (pesquisaEmpresa.SubscricaoAtiva != null)
            {
                if (pesquisaEmpresa.SubscricaoAtiva.Equals("subscricaoTrue"))
                {
                    task = task.Where(e => e.EstadoSubscricao);
                }
                else if (pesquisaEmpresa.SubscricaoAtiva.Equals("subscricaoFalse"))
                {
                    task = task.Where(e => !e.EstadoSubscricao);
                }
            }
                
            if (pesquisaEmpresa.Ordem != null)
            {
                if (pesquisaEmpresa.Ordem.Equals("nomeDesc"))
                {
                    task = task.OrderByDescending(e => e.Nome);
                }
                else if (pesquisaEmpresa.Ordem.Equals("nomeAsc"))
                {
                    task = task.OrderBy(e => e.Nome);
                }
                else if (pesquisaEmpresa.Ordem.Equals("classDesc"))
                {
                    task = task.OrderByDescending(e => e.Classificacoes.Sum(c => c.ClassificacaoReserva)/(e.Classificacoes.Count == 0 ? 1 : e.Classificacoes.Count));
                }
                else if (pesquisaEmpresa.Ordem.Equals("classAsc"))
                {
                    task = task.OrderBy(e => e.Classificacoes.Sum(c => c.ClassificacaoReserva)/(e.Classificacoes.Count == 0 ? 1 : e.Classificacoes.Count));
                }
            } else
            {
                task = task.OrderByDescending(e => e.Nome);
            }
            
            pesquisaEmpresa.ListaDeEmpresas = await task.ToListAsync();

            return View(pesquisaEmpresa);
        }
        
        // GET: Empresas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empresas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,EstadoSubscricao,Classificacao")] Empresa empresa)
        {
            ModelState.Remove("Veiculos");
            ModelState.Remove("Trabalhadores");
            if (ModelState.IsValid)
            {
                _context.Add(empresa);
                await _context.SaveChangesAsync();

                var defaultUser = new ApplicationUser
                {
                    UserName = "gestor@" + String.Concat(empresa.Nome.ToLower().Where(c => !Char.IsWhiteSpace(c))) + ".com",
                    Email = "gestor@" + String.Concat(empresa.Nome.ToLower().Where(c => !Char.IsWhiteSpace(c))) + ".com",
                    PrimeiroNome = "Gestor",
                    UltimoNome = empresa.Nome,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    EmpresaId = empresa.Id,
                    ContaAtiva = true
                };

                var user = await _userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await _userManager.CreateAsync(defaultUser, "1qazZAQ!");
                    await _userManager.AddToRoleAsync(defaultUser,
                    Roles.Gestor.ToString());
                }

                return RedirectToAction(nameof(Index));
            }
            return View(empresa);
        }

        // GET: Ativar
        public async Task<IActionResult> Ativar(int? id)
        {
            if (id == null || _context.Empresa == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }

            empresa.EstadoSubscricao = true;
            _context.Update(empresa);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Desativar
        public async Task<IActionResult> Desativar(int? id)
        {
            if (id == null || _context.Empresa == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }

            empresa.EstadoSubscricao = false;
            _context.Update(empresa);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Empresas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Empresa == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresa.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            return View(empresa);
        }

        // POST: Empresas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,EstadoSubscricao,Classificacao")] Empresa empresa)
        {
            if (id != empresa.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Veiculos");
            ModelState.Remove("Trabalhadores");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaExists(empresa.Id))
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
            return View(empresa);
        }

        // GET: Empresas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Empresa == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresa
                .Include(e => e.Classificacoes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }

            if (_context.Veiculo.Where(v => v.EmpresaId== id).Any())
            {
                TempData["error"] = "A empresa " + empresa.Nome + " ainda tem veiculos para aluguer.";
                return RedirectToAction(nameof(Index));
            }

            return View(empresa);
        }

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Empresa == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Empresa'  is null.");
            }

            var empresa = await _context.Empresa.FindAsync(id);
            if (empresa != null)
            {
                var users = await _userManager.Users.Where(u => u.EmpresaId == id).ToListAsync();

                foreach (var user in users)
                {
                    await _userManager.DeleteAsync(user);
                }
                _context.Empresa.Remove(empresa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpresaExists(int id)
        {
          return _context.Empresa.Any(e => e.Id == id);
        }
    }
}
