using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TrabalhoPratico.Data;
using TrabalhoPratico.Models;
using TrabalhoPratico.Models.ViewModels;

namespace TrabalhoPratico.Controllers
{
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
        [Authorize(Roles = "Funcionario,Gestor")]
        public async Task<IActionResult> Index(
            [Bind("TextoAPesquisar,Ordem,CategoriaId,Disponivel")] PesquisaFrotaVeiculosViewModel pesquisaVeiculos,
            string Categoria)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
                TempData.Remove("error");
            }

            var user = await _userManager.GetUserAsync(User);

            IQueryable<Veiculo> task = _context.Veiculo.Include(v => v.Categoria)
                .Include(v => v.Empresa).Include(v => v.Localizacao).Where(v => v.EmpresaId == user.EmpresaId);
            if (string.IsNullOrWhiteSpace(pesquisaVeiculos.TextoAPesquisar))
            {
                task = task.OrderByDescending(v => v.Marca).ThenByDescending(v => v.Modelo).ThenByDescending(v => v.Ano);
            }
            else
            {
                task = task
                    .Where(e => 
                        e.Marca.Contains(pesquisaVeiculos.TextoAPesquisar)
                        || e.Modelo.Contains(pesquisaVeiculos.TextoAPesquisar)
                        || e.Matricula.Contains(pesquisaVeiculos.TextoAPesquisar)
                        || e.Ano.ToString().Contains(pesquisaVeiculos.TextoAPesquisar));
            }
            if (Categoria != null)
            {
                pesquisaVeiculos.CategoriaId = int.Parse(Categoria);
            }
            if (pesquisaVeiculos.CategoriaId != 0)
            {
                task = task.Where(e => e.CategoriaId == pesquisaVeiculos.CategoriaId);
            }

            if (pesquisaVeiculos.Disponivel != null)
            {
                if (pesquisaVeiculos.Disponivel.Equals("disponivelSim"))
                {
                    task = task.Where(e => e.Disponivel);
                }
                else if (pesquisaVeiculos.Disponivel.Equals("disponivelNao"))
                {
                    task = task.Where(e => !e.Disponivel);
                }
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

            ViewBag.CategoriaId = _context.CategoriaVeiculo.ToList();

            pesquisaVeiculos.ListaDeVeiculos = await task.ToListAsync();

            return View(pesquisaVeiculos);
        }

        // GET: Veiculos/Details/5
        [Authorize(Roles = "Funcionario,Gestor")]
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
                .Include(v => v.Empresa.Classificacoes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }

            return View(veiculo);
        }

        // GET: Ativar
        [Authorize(Roles = "Funcionario,Gestor")]
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
        [Authorize(Roles = "Funcionario,Gestor")]
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
        [Authorize(Roles = "Funcionario,Gestor")]
        public IActionResult Create()
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
                TempData.Remove("error");
            }
            ViewData["CategoriaId"] = new SelectList(_context.CategoriaVeiculo, "Id", "Nome");
            ViewBag.LocalizacaoId = new SelectList(_context.Localizacao, "Id", "Nome");
            return View(new VeiculoViewModel());
        }

        // POST: Veiculos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Funcionario,Gestor")]
        public async Task<IActionResult> Create([Bind("Id,Marca,Modelo,Ano,Matricula,Quilometros,Disponivel,PrecoDia,CategoriaId,LocalizacaoId,Avatar,AvatarFile")] VeiculoViewModel veiculo)
        {
            veiculo.EmpresaId = (int)(await _userManager.GetUserAsync(User)).EmpresaId;

            ModelState.Remove("Categoria");
            ModelState.Remove("Localizacao");
            ModelState.Remove("Empresa");
            if (ModelState.IsValid)
            {
                if (veiculo.AvatarFile != null)
                {
                    if (veiculo.AvatarFile.Length > (200 * 1024))
                    {
                        TempData["error"] = "Error: Ficheiro demasiado grande";
                        return View(veiculo);
                    }
                    // método a implementar – verifica se a extensão é .png,.jpg,.jpeg
                    if (!isValidFileType(veiculo.AvatarFile.FileName))
                    {
                        TempData["error"] = "Error: Ficheiro não suportado";
                        return View(veiculo);
                    }
                    using (var dataStream = new MemoryStream())
                    {
                        await veiculo.AvatarFile.CopyToAsync(dataStream);
                        veiculo.Avatar = dataStream.ToArray();
                    }
                }

                Veiculo v = new Veiculo()
                {
                    Marca= veiculo.Marca,
                    Modelo= veiculo.Modelo,
                    Ano= veiculo.Ano,
                    Matricula= veiculo.Matricula,
                    Quilometros= veiculo.Quilometros,
                    Disponivel= veiculo.Disponivel,
                    PrecoDia= veiculo.PrecoDia,
                    CategoriaId= veiculo.CategoriaId,
                    LocalizacaoId= veiculo.LocalizacaoId,
                    Avatar= veiculo.Avatar,
                    EmpresaId= veiculo.EmpresaId,
                };

                _context.Add(v);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoriaId = new SelectList(_context.CategoriaVeiculo, "Id", "Nome", veiculo.CategoriaId);
            ViewBag.LocalizacaoId = new SelectList(_context.Localizacao, "Id", "Nome", veiculo.LocalizacaoId);
            return View(veiculo);
        }


        //verifica se a extensão é .png,.jpg,.jpeg
        public bool isValidFileType(string filename)
        {
            List<string> fileExtensions = new List<string>() { "png", "jpg", "jpeg" };
            List<string> filenameSeparated = filename.Split('.').Reverse().ToList<string>();

            foreach (var extension in fileExtensions)
                if (extension.Equals(filenameSeparated[0]))
                    return true;

            return false;
        }

        // GET: Veiculos/Edit/5
        [Authorize(Roles = "Funcionario,Gestor")]
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

            VeiculoViewModel v = new VeiculoViewModel()
            {
                Marca= veiculo.Marca,
                Modelo= veiculo.Modelo,
                Ano= veiculo.Ano,
                Matricula= veiculo.Matricula,
                Quilometros= veiculo.Quilometros,
                Disponivel= veiculo.Disponivel,
                PrecoDia= veiculo.PrecoDia,
                CategoriaId= veiculo.CategoriaId,
                LocalizacaoId= veiculo.LocalizacaoId,
                Avatar= veiculo.Avatar,
                EmpresaId= veiculo.EmpresaId,
            };

            ViewData["CategoriaId"] = new SelectList(_context.CategoriaVeiculo, "Id", "Nome", veiculo.CategoriaId);
            ViewData["LocalizacaoId"] = new SelectList(_context.Localizacao, "Id", "Nome", veiculo.LocalizacaoId);
            return View(v);
        }

        // POST: Veiculos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Funcionario,Gestor")]
        public async Task<IActionResult> Edit(
            int id, 
            [Bind("Id,Marca,Modelo,Ano,Matricula,Quilometros,Disponivel,PrecoDia,CategoriaId,LocalizacaoId,Avatar,AvatarFile")] VeiculoViewModel veiculo)
        {
            ViewData["CategoriaId"] = new SelectList(_context.CategoriaVeiculo, "Id", "Nome", veiculo.CategoriaId);
            ViewData["LocalizacaoId"] = new SelectList(_context.Localizacao, "Id", "Nome", veiculo.LocalizacaoId);

            var veiculoDB = _context.Veiculo.Where(v=> id == v.Id).First();
            veiculo.Avatar = veiculoDB.Avatar;
            _context.Entry(veiculoDB).State = EntityState.Detached;

            if (id != veiculo.Id)
            {
                return NotFound();
            }
            veiculo.EmpresaId = (int)(await _userManager.GetUserAsync(User)).EmpresaId;

            ModelState.Remove("AvatarFile");
            ModelState.Remove("Categoria");
            ModelState.Remove("Localizacao");
            ModelState.Remove("Empresa");
            if (ModelState.IsValid)
            {
                try
                {
                    if (veiculo.AvatarFile != null)
                    {
                        if (veiculo.AvatarFile.Length > (200 * 1024))
                        {
                            ViewData["error"] = "Error: Ficheiro demasiado grande";
                            return View(veiculo);
                        }
                        // método a implementar – verifica se a extensão é .png,.jpg,.jpeg
                        if (!isValidFileType(veiculo.AvatarFile.FileName))
                        {
                            ViewData["error"] = "Error: Ficheiro não suportado";
                            return View(veiculo);
                        }
                        using (var dataStream = new MemoryStream())
                        {
                            await veiculo.AvatarFile.CopyToAsync(dataStream);
                            veiculo.Avatar = dataStream.ToArray();
                        }
                    }

                    Veiculo v = new Veiculo()
                    {
                        Id = veiculo.Id,
                        Marca= veiculo.Marca,
                        Modelo= veiculo.Modelo,
                        Ano= veiculo.Ano,
                        Matricula= veiculo.Matricula,
                        Quilometros= veiculo.Quilometros,
                        Disponivel= veiculo.Disponivel,
                        PrecoDia= veiculo.PrecoDia,
                        CategoriaId= veiculo.CategoriaId,
                        LocalizacaoId= veiculo.LocalizacaoId,
                        Avatar = veiculo.Avatar,
                        EmpresaId= veiculo.EmpresaId,
                    };

                    _context.Update(v);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Veiculo.Any(e => e.Id == id))
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
            return View(veiculo);
        }

        // GET: Veiculos/Delete/5
        [Authorize(Roles = "Funcionario,Gestor")]
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
        [Authorize(Roles = "Funcionario,Gestor")]
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
    
        public async Task<IActionResult> Search(
            [Bind("TextoAPesquisar,Ordem,CategoriaId,EmpresaId,DataLevantamento,DataEntrega")] PesquisaVeiculosViewModel pesquisaVeiculos,
            string Categoria, string Empresa)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
                TempData.Remove("error");
            }

            if (pesquisaVeiculos.DataLevantamento == DateTime.Parse("01/01/0001 00:00:00"))
            {
                pesquisaVeiculos.DataLevantamento = DateTime.Now;
            }
            if (pesquisaVeiculos.DataEntrega == DateTime.Parse("01/01/0001 00:00:00"))
            {
                pesquisaVeiculos.DataEntrega = DateTime.Now;
            }

            IQueryable<Veiculo> task = _context.Veiculo
                .Include(v => v.Categoria)
                .Include(v => v.Empresa)
                .Include(v => v.Empresa.Classificacoes)
                .Include(v => v.Localizacao)
                .Where(v => v.Disponivel && v.Empresa.EstadoSubscricao);
            if (string.IsNullOrEmpty(pesquisaVeiculos.TextoAPesquisar))
            {
                task = task.OrderBy(v => v.PrecoDia);
            }
            else
            {
                task = task
                    .Where(e => (
                        e.Marca.Contains(pesquisaVeiculos.TextoAPesquisar)
                        || e.Modelo.Contains(pesquisaVeiculos.TextoAPesquisar)
                        || e.Localizacao.Nome.Contains(pesquisaVeiculos.TextoAPesquisar)
                    ));

                

                if (pesquisaVeiculos.CategoriaId != 0)
                {
                    task = task.Where(e => e.CategoriaId == pesquisaVeiculos.CategoriaId);
                }
            }

            if (Categoria != null)
            {
                pesquisaVeiculos.CategoriaId = int.Parse(Categoria);
            }
            if (pesquisaVeiculos.CategoriaId != 0)
            {
                task = task.Where(r => r.CategoriaId == pesquisaVeiculos.CategoriaId);
            }

            if (Empresa != null)
            {
                pesquisaVeiculos.EmpresaId = int.Parse(Empresa);
            }
            if (pesquisaVeiculos.EmpresaId != 0)
            {
                task = task.Where(r => r.EmpresaId == pesquisaVeiculos.EmpresaId);
            }

            if (pesquisaVeiculos.Ordem != null)
            {
                if (pesquisaVeiculos.Ordem.Equals("precoDesc"))
                {
                    task = task.OrderByDescending(v => v.PrecoDia);
                }
                else if (pesquisaVeiculos.Ordem.Equals("precoAsc"))
                {
                    task = task.OrderBy(v => v.PrecoDia);
                }
                else if (pesquisaVeiculos.Ordem.Equals("classDesc"))
                {
                    //task = task.OrderByDescending(v => v.Empresa.Classificacao);
                }
            }

            ViewBag.CategoriaId = _context.CategoriaVeiculo.ToList();
            ViewBag.EmpresaId = _context.Empresa.ToList();

            pesquisaVeiculos.ListaDeVeiculos = await task.ToListAsync();

            foreach(var veiculo in pesquisaVeiculos.ListaDeVeiculos.ToList())
            {
                var res = await _context.Reserva.Include(r => r.Veiculo).Where(r => r.Veiculo.Id == veiculo.Id
                && ((DateTime.Compare(r.DataLevantamento, pesquisaVeiculos.DataLevantamento) >= 0
                && DateTime.Compare(r.DataLevantamento, pesquisaVeiculos.DataEntrega) <= 0)
                || (DateTime.Compare(r.DataEntrega, pesquisaVeiculos.DataLevantamento) >= 0
                && DateTime.Compare(r.DataEntrega, pesquisaVeiculos.DataEntrega) <= 0))
                ).FirstOrDefaultAsync();

                if(res != null)
                {
                    pesquisaVeiculos.ListaDeVeiculos.Remove(veiculo);
                }
            }

            if(pesquisaVeiculos.DataEntrega < pesquisaVeiculos.DataLevantamento)
            {
                ViewBag.error = "Datas de levantamento e entrega incorretas";
            }

            return View(pesquisaVeiculos);
        }
    }
}
