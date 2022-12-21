using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using TrabalhoPratico.Data;
using TrabalhoPratico.Models;
using TrabalhoPratico.Models.ViewModels;

namespace TrabalhoPratico.Controllers
{
    public class UtilizadoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UtilizadoresController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
    }

        // GET: Utilizadores
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index([Bind("TextoAPesquisar,Ordem")] PesquisaUtilizadorViewModel pesquisaUtilizador)
        {

            IQueryable<ApplicationUser> task;
            if (string.IsNullOrWhiteSpace(pesquisaUtilizador.TextoAPesquisar))
            {
                task = _userManager.Users.Where(e => e.PrimeiroNome.Contains(""))
                    .OrderByDescending(e => e.PrimeiroNome)
                    .ThenByDescending(e => e.UltimoNome)
                    .ThenByDescending(e => e.UserName);
            }
            else
            {
                task = _userManager.Users.Where(e => 
                e.PrimeiroNome.Contains(pesquisaUtilizador.TextoAPesquisar)
                || e.UltimoNome.Contains(pesquisaUtilizador.TextoAPesquisar)
                || e.UserName.Contains(pesquisaUtilizador.TextoAPesquisar)
                );
            }
            if (pesquisaUtilizador.Ordem != null)
            {
                if (pesquisaUtilizador.Ordem.Equals("nomeDesc"))
                {
                    task = task.OrderByDescending(e => e.PrimeiroNome)
                        .ThenByDescending(e => e.UltimoNome)
                        .ThenByDescending(e => e.UserName);
                }
                else if (pesquisaUtilizador.Ordem.Equals("nomeAsc"))
                {
                    task = task.OrderBy(e => e.PrimeiroNome)
                        .ThenBy(e => e.UltimoNome)
                        .ThenBy(e => e.UserName);
                }
            }

            var listaUsers = await task.ToListAsync();

            var rolesPesquisa = await _context.Roles.Where(r => r.Name.Contains(pesquisaUtilizador.TextoAPesquisar)).ToListAsync();

            foreach(var role in rolesPesquisa)
            {
                var usersInRoles = await _userManager.GetUsersInRoleAsync(role.Name);
            
                foreach(var user in usersInRoles)
                {
                    if (!listaUsers.Contains(user))
                    {
                        listaUsers.Add(user);
                    }
                }
            }

            List<UtilizadorViewModel> usersViewModel = new List<UtilizadorViewModel>();

            foreach (var user in listaUsers)
            {
                UtilizadorViewModel userRolesViewModel = new UtilizadorViewModel();

                userRolesViewModel.Id = user.Id;
                userRolesViewModel.UserName = user.UserName;
                userRolesViewModel.PrimeiroNome = user.PrimeiroNome;
                userRolesViewModel.UltimoNome = user.UltimoNome;
                userRolesViewModel.Ativo = user.ContaAtiva;

                userRolesViewModel.Roles = await _userManager.GetRolesAsync(user);

                usersViewModel.Add(userRolesViewModel);
            }

            pesquisaUtilizador.ListaDeUtilizadores = usersViewModel;

            return View(pesquisaUtilizador);
        }

        // GET: Ativar
        [Authorize(Roles = "Administrador,Gestor")]
        public async Task<IActionResult> Ativar(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utilizador == null)
            {
                return NotFound();
            }

            utilizador.ContaAtiva = true;
            await _userManager.UpdateAsync(utilizador);

            var admins = await _userManager.GetUsersInRoleAsync("Administrador");
            if(admins.Contains(await _userManager.GetUserAsync(User)))
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Gestor));
        }

        // GET: Desativar
        [Authorize(Roles = "Administrador,Gestor")]
        public async Task<IActionResult> Desativar(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utilizador == null)
            {
                return NotFound();
            }

            utilizador.ContaAtiva = false;
            await _userManager.UpdateAsync(utilizador);

            var admins = await _userManager.GetUsersInRoleAsync("Administrador");
            if (admins.Contains(await _userManager.GetUserAsync(User)))
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Gestor));
        }

        // GET: Utilizadores/Edit/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Users.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound();
            }
            return View(utilizador);
        }

        // POST: Utilizadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(string id, [Bind("Id,PrimeiroNome,UltimoNome,NIF,DataNascimento")] ApplicationUser user)
        {

            var utilizador = await _userManager.Users.Where(u => u.Id == user.Id).FirstAsync();

            if (id != user.Id || utilizador == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if(utilizador.PrimeiroNome != user.PrimeiroNome)
                {
                    utilizador.PrimeiroNome = user.PrimeiroNome;
                    await _userManager.UpdateAsync(utilizador);
                }
                if(utilizador.UltimoNome != user.UltimoNome)
                {
                    utilizador.UltimoNome = user.UltimoNome;
                    await _userManager.UpdateAsync(utilizador);
                }
                if(utilizador.NIF != user.NIF)
                {
                    utilizador.NIF = user.NIF;
                    await _userManager.UpdateAsync(utilizador);
                }
                if(utilizador.DataNascimento != user.DataNascimento)
                {
                    utilizador.DataNascimento = user.DataNascimento;
                    await _userManager.UpdateAsync(utilizador);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
    
        // GET: Utilizadores
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Gestor([Bind("TextoAPesquisar,Ordem")] PesquisaUtilizadorViewModel pesquisaUtilizador)
        {

            var userLogado = await _userManager.GetUserAsync(User);

            IQueryable<ApplicationUser> task;
            if (string.IsNullOrWhiteSpace(pesquisaUtilizador.TextoAPesquisar))
            {
                task = _userManager.Users.Where(e => e.PrimeiroNome.Contains("") && e.EmpresaId == userLogado.EmpresaId)
                    .OrderByDescending(e => e.PrimeiroNome)
                    .ThenByDescending(e => e.UltimoNome)
                    .ThenByDescending(e => e.UserName);
            }
            else
            {
                task = _userManager.Users.Where(e =>
                e.EmpresaId == userLogado.EmpresaId

                && (e.PrimeiroNome.Contains(pesquisaUtilizador.TextoAPesquisar)
                || e.UltimoNome.Contains(pesquisaUtilizador.TextoAPesquisar)
                || e.UserName.Contains(pesquisaUtilizador.TextoAPesquisar))
                );
            }
            if (pesquisaUtilizador.Ordem != null)
            {
                if (pesquisaUtilizador.Ordem.Equals("nomeDesc"))
                {
                    task = task.OrderByDescending(e => e.PrimeiroNome)
                        .ThenByDescending(e => e.UltimoNome)
                        .ThenByDescending(e => e.UserName);
                }
                else if (pesquisaUtilizador.Ordem.Equals("nomeAsc"))
                {
                    task = task.OrderBy(e => e.PrimeiroNome)
                        .ThenBy(e => e.UltimoNome)
                        .ThenBy(e => e.UserName);
                }
            }

            var listaUsers = await task.ToListAsync();

            var rolesPesquisa = await _context.Roles.Where(r => r.Name.Contains(pesquisaUtilizador.TextoAPesquisar)).ToListAsync();

            foreach (var role in rolesPesquisa)
            {
                var usersInRoles = await _userManager.GetUsersInRoleAsync(role.Name);

                foreach (var userInRole in usersInRoles)
                {
                    if (!listaUsers.Contains(userInRole) && userInRole.EmpresaId == userLogado.EmpresaId)
                    {
                        listaUsers.Add(userInRole);
                    }
                }
            }

            listaUsers.Remove(userLogado);

            List<UtilizadorViewModel> usersViewModel = new List<UtilizadorViewModel>();

            foreach (var user in listaUsers)
            {
                UtilizadorViewModel userRolesViewModel = new UtilizadorViewModel();

                userRolesViewModel.Id = user.Id;
                userRolesViewModel.UserName = user.UserName;
                userRolesViewModel.PrimeiroNome = user.PrimeiroNome;
                userRolesViewModel.UltimoNome = user.UltimoNome;
                userRolesViewModel.Ativo = user.ContaAtiva;

                userRolesViewModel.Roles = await _userManager.GetRolesAsync(user);

                usersViewModel.Add(userRolesViewModel);
            }

            pesquisaUtilizador.ListaDeUtilizadores = usersViewModel;

            return View(pesquisaUtilizador);
        }

        // GET: Create
        [Authorize(Roles = "Gestor")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reservas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Create([Bind("Email,PrimeiroNome,UltimoNome,DataNascimento,NIF,TipoUser,Password,ConfirmPassword")] AdicionarFuncionarioGestorViewModel utilizador)
        {

            if (ModelState.IsValid)
            {

                var userLogado = await _userManager.GetUserAsync(User);
                
                var newUser = new ApplicationUser
                {
                    UserName = utilizador.Email,
                    Email = utilizador.Email,
                    PrimeiroNome = utilizador.PrimeiroNome,
                    UltimoNome = utilizador.UltimoNome,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    ContaAtiva = true
                };

                newUser.EmpresaId = (await _context.Users.Include(u => u.Empresa).Where(u => u.Id == userLogado.Id).FirstOrDefaultAsync()).Empresa.Id;

                var user = await _userManager.FindByEmailAsync(newUser.Email);
                if (user == null)
                {
                    await _userManager.CreateAsync(newUser, utilizador.Password);
                    if (utilizador.TipoUser.Equals("func"))
                    {
                        await _userManager.AddToRoleAsync(newUser, Roles.Funcionario.ToString());
                    }
                    else if (utilizador.TipoUser.Equals("gestor"))
                    {
                        await _userManager.AddToRoleAsync(newUser, Roles.Gestor.ToString());
                    }
                }
                
                return RedirectToAction(nameof(Gestor));
            }

            return View(utilizador);
        }
    }
}
