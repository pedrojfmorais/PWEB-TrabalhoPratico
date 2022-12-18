using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TrabalhoPratico.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }

        [PersonalData, DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [PersonalData]
        [RegularExpression("^\\d{9}$", ErrorMessage = "O NIF tem de ter 9 digitos!")]
        public int NIF { get; set; }

        public bool ContaAtiva { get; set; }

        public int? EmpresaId { get; set; }
        public Empresa? Empresa { get; set; }
    }
}
