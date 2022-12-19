using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TrabalhoPratico.Models
{
    public class CategoriaVeiculo
    {
        public int Id { get; set; }

        [Display(Name = "Nome", Prompt = "Introduza o nome da categoria",
            Description = "Nome da nova categoria a inserir")]
        public string Nome { get; set; }

        public ICollection<Veiculo> Veiculos { get; set; }
    }
}
