using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TrabalhoPratico.Models.ViewModels
{
    public class UtilizadorViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }
        public bool Ativo { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
