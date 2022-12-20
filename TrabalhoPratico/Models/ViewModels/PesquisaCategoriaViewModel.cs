using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TrabalhoPratico.Models.ViewModels
{
    public class PesquisaCategoriaViewModel
    {
        public List<CategoriaVeiculo> ListaDeCategorias { get; set; }
        [Display(Name = "Pesquisa de categorias:", Prompt = "introduza o nome a pesquisar")]
        public string TextoAPesquisar { get; set; }
        public string Ordem { get; set; }
    }
}
