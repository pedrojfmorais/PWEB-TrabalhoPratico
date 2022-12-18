using System.ComponentModel.DataAnnotations;

namespace TrabalhoPratico.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime DataLevantamento { get; set; }
        [DataType(DataType.Date)]
        public DateTime DataEntrega { get; set; }

        public int VeiculoId { get; set; }
        public Veiculo Veiculo { get; set; }

        public string ClienteId { get; set; }
        public ApplicationUser Cliente { get; set; }

        // estado levantamento
        // estado entrega
    }
}
