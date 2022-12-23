using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrabalhoPratico.Models
{
    public class Classificacao
    {
        public int Id { get; set; }

        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        [ForeignKey(nameof(Reserva))]
        public int? ReservaId { get; set; }
        public Reserva? Reserva { get; set; }

        [Display(Name = "Classificação", Prompt = "Introduza a classificação",
            Description = "Classificação da reserva que realizou")]
        [RegularExpression("^(10|[0-9])$", ErrorMessage = "O valor inserido tem de ser entre 0 e 10!")]
        public int ClassificacaoReserva{ get; set; }
    
    }
}
