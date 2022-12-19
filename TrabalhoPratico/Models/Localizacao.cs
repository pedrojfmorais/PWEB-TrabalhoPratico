using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TrabalhoPratico.Models
{
    public class Localizacao
    {
        public int Id { get; set; }

        [Display(Name = "Nome", Prompt = "Introduza o nome da localizacao",
            Description = "Nome da nova localizacao a inserir")]
        public string Nome { get; set; }

        public ICollection<Veiculo> Veiculos { get; set; }
    }
}
