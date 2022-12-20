using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrabalhoPratico.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        [Display(Name = "Data de Levantamento", Prompt = "Introduza data de levantamento",
            Description = "Data de levantamento do veiculo")]
        [DataType(DataType.Date)]
        public DateTime DataLevantamento { get; set; }

        [Display(Name = "Data de Entrega", Prompt = "Introduza data de entrega",
            Description = "Data de entrega do veiculo")]
        [DataType(DataType.Date)]
        public DateTime DataEntrega { get; set; }

        [Display(Name = "Reserva Confirmada", Description = "Se a reserva já foi confirmada por um trabalhador")]
        public bool Confirmada { get; set; }

        public int VeiculoId { get; set; }
        public Veiculo Veiculo { get; set; }

        public string ClienteId { get; set; }
        public ApplicationUser Cliente { get; set; }
        
        public int? ReservaEstadoVeiculoLevantamentoId { get; set; }
        public ReservaEstadoVeiculoLevantamento? ReservaEstadoVeiculoLevantamento { get; set; }

        public int? ReservaEstadoVeiculoEntregaId { get; set; }
        public ReservaEstadoVeiculoEntrega? ReservaEstadoVeiculoEntrega { get; set; }
    }
}
