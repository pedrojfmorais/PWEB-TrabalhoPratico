using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TrabalhoPratico.Models.ViewModels
{
    public class PesquisaLocalizacaoViewModel
    {
        public List<Localizacao> ListaDeLocalizacoes { get; set; }
        [Display(Name = "Pesquisa de Localizações:", Prompt = "introduza o nome a pesquisar")]
        public string TextoAPesquisar { get; set; }
        public string Ordem { get; set; }
    }
}
