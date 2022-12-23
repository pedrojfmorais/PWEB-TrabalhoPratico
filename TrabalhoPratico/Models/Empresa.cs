using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TrabalhoPratico.Models
{
    public class Empresa
    {
        public int Id { get; set; }

        [Display(Name = "Nome", Prompt = "Introduza o nome da empresa",
            Description = "Nome da nova empresa a inserir")]
        public string Nome { get; set; }

        [Display(Name = "Estado Subscrição", Prompt = "Introduza o estado da subscrição",
            Description = "Estado da subscrição da nova empresa a inserir")]
        public bool EstadoSubscricao { get; set; }

        public ICollection<Classificacao> Classificacoes { get; set; }
        public ICollection<Veiculo> Veiculos { get; set; }
        public ICollection<ApplicationUser> Trabalhadores { get; set; }

    }
}
