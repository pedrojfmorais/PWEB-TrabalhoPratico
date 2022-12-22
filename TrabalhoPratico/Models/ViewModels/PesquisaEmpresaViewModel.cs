using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TrabalhoPratico.Models.ViewModels
{
    public class PesquisaEmpresaViewModel
    {
        public List<Empresa> ListaDeEmpresas { get; set; }
        [Display(Name = "Pesquisa de empresas:", Prompt = "introduza o nome a pesquisar")]
        public string TextoAPesquisar { get; set; }
        public string SubscricaoAtiva { get; set; }
        public string Ordem { get; set; }
    }
}
