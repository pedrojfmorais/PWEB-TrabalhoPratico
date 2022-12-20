using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TrabalhoPratico.Models.ViewModels
{
    public class PesquisaVeiculosViewModel
    {
        public List<Veiculo> ListaDeVeiculos { get; set; }
        [Display(Name = "Pesquisa de veiculos:", Prompt = "introduza o veiculo a pesquisar")]
        public string TextoAPesquisar { get; set; }
        public int CategoriaId { get; set; }
        public int EmpresaId { get; set; }
        public string Ordem { get; set; }
    }
}
