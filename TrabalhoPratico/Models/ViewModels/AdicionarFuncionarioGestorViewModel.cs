using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TrabalhoPratico.Models.ViewModels
{
    public class AdicionarFuncionarioGestorViewModel
    {
        [Display(Name = "Email", Prompt = "Introduza o eamil",
            Description = "Email do utilizador")]
        public string Email { get; set; }
        
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

        [Display(Name = "Tipo de funcionário")]
        public string TipoUser { get; set; }

        [Display(Name = "Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [Required(ErrorMessage = "Password é obrigatória")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirmar Password")]
        [Required(ErrorMessage = "Confirmar Password é obrigatória")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
