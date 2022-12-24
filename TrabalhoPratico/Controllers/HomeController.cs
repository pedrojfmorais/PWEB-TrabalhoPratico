using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TrabalhoPratico.Data;
using TrabalhoPratico.Models;

namespace TrabalhoPratico.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context= context;
        }

        public async Task<IActionResult> IndexAsync()
        {

            List<Veiculo> veiculos = await _context.Veiculo
               .Include(v => v.Categoria)
               .Include(v => v.Empresa)
               .Include(v => v.Empresa.Classificacoes)
               .Include(v => v.Localizacao)
               .Where(v => v.Disponivel && v.Empresa.EstadoSubscricao).OrderBy(v => v.PrecoDia).ToListAsync();

            return View(veiculos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}