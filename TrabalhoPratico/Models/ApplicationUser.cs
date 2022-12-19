using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace TrabalhoPratico.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Primeiro Nome", Prompt = "Introduza o primeiro nome",
            Description = "Primeiro nome do utilizador")]
        public string PrimeiroNome { get; set; }

        [Display(Name = "Último Nome", Prompt = "Introduza o último nome",
            Description = "Último nome do utilizador")]
        public string UltimoNome { get; set; }

        [Display(Name = "Data de Nascimento", Prompt = "Introduza a data se nascimento",
            Description = "Data se nascimento do utilizador")]
        [PersonalData, DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [Display(Name = "NIF", Prompt = "Introduza o NIF",
            Description = "Número de Indentificação Fiscal do utilizador")]
        [PersonalData]
        [RegularExpression("^\\d{9}$", ErrorMessage = "O NIF tem de ter 9 digitos!")]
        public int NIF { get; set; }

        [Display(Name = "Conta de utilizador ativa", Description = "Se a conta de utilizador está ativa")]
        public bool ContaAtiva { get; set; }

        public int? EmpresaId { get; set; }
        public Empresa? Empresa { get; set; }
        public ICollection<ReservaEstadoVeiculoLevantamento> VeiculosEntreguesAClientes { get; set; }
    }
}
