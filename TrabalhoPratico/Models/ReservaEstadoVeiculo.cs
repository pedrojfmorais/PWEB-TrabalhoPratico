using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace TrabalhoPratico.Models
{
    public abstract class ReservaEstadoVeiculo
    {
        public int Id { get; set; }

        [Display(Name = "Quilometros", Prompt = "Introduza os quilometros atuais do veiculo",
            Description = "Quilometros do veiculo")]
        public int Quilometros { get; set; }

        [Display(Name = "Danos do Veiculo", Prompt = "O veiculo tem danos?",
            Description = "Se o veiculo tem danos")]
        public bool DanosVeiculo { get; set; }

        [Display(Name = "Observações", Prompt = "Introduza os observações sobre a reserva",
            Description = "Quilometros do veiculo")]
        [DataType(DataType.MultilineText)]
        public string Observacoes { get; set; }

        public string FuncionarioId { get; set; }
        public ApplicationUser Funcionario { get; set; }

        [ForeignKey(nameof(Reserva))]
        public int? ReservaId { get; set; }
        public Reserva? Reserva { get; set; }
    }

    [Table("ReservaEstadoVeiculoLevantamento")]
    public class ReservaEstadoVeiculoLevantamento : ReservaEstadoVeiculo
    {
    }

    [Table("ReservaEstadoVeiculoEntrega")]
    public class ReservaEstadoVeiculoEntrega : ReservaEstadoVeiculo
    {
    }

}
