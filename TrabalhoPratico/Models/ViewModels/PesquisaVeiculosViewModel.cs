using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TrabalhoPratico.Models.ViewModels
{
    public class PesquisaVeiculosViewModel
    {
        public List<Veiculo> ListaDeVeiculos { get; set; }

        [Display(Name = "Pesquisa de veiculos:", Prompt = "introduza o veiculo a pesquisar")]
        public string TextoAPesquisar { get; set; }

        [Display(Name = "Data de Levantamento", Prompt = "Introduza data de levantamento",
            Description = "Data de levantamento do veiculo")]
        [DataType(DataType.Date)]
        public DateTime DataLevantamento { get; set; }

        [Display(Name = "Data de Entrega", Prompt = "Introduza data de entrega",
            Description = "Data de entrega do veiculo")]
        [DataType(DataType.Date)]
        public DateTime DataEntrega { get; set; }

        public int CategoriaId { get; set; }
        public int EmpresaId { get; set; }
        public string Ordem { get; set; }
    }
}
