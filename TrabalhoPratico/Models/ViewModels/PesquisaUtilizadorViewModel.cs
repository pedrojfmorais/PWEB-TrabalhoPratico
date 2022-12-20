using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TrabalhoPratico.Models.ViewModels
{
    public class PesquisaUtilizadorViewModel
    {
        public List<UtilizadorViewModel> ListaDeUtilizadores { get; set; }
        [Display(Name = "Pesquisa de utilizadores:", Prompt = "introduza o texto a pesquisar")]
        public string TextoAPesquisar { get; set; }
        public string Ordem { get; set; }
    }
}
