using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TrabalhoPratico.Models.ViewModels
{
    public class VeiculoViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Foto veiculo")]
        public byte[]? Avatar { get; set; }
        public IFormFile AvatarFile { get; set; }

        [Display(Name = "Marca", Prompt = "Introduza a marca do veiculo",
            Description = "Marca do novo veiculo a inserir")]
        public string Marca { get; set; }

        [Display(Name = "Modelo", Prompt = "Introduza o modelo do veiculo",
            Description = "Modelo do novo veiculo a inserir")]
        public string Modelo { get; set; }

        [Display(Name = "Ano", Prompt = "Introduza o ano do veiculo",
            Description = "Ano do novo veiculo a inserir")]
        public int Ano { get; set; }

        [Display(Name = "Matricula", Prompt = "Introduza a matricula do veiculo",
            Description = "Matricula do novo veiculo a inserir")]
        public string Matricula { get; set; }

        [Display(Name = "Quilometros", Prompt = "Introduza os quilometros do veiculo",
            Description = "Quilometros do novo veiculo a inserir")]
        public int Quilometros { get; set; }

        [Display(Name = "Disponivel", Prompt = "Disponivel para para reservar",
            Description = "Novo veiculo está disponivel para alugar")]
        public bool Disponivel { get; set; }

        [Display(Name = "Preço dia", Prompt = "Preço por dia de aluguer do veiculo",
            Description = "Preço por dia de aluguer do novo veiculo a inserir")]
        public decimal PrecoDia { get; set;}

        [Display(Name = "Categoria", Prompt = "Insira a categoria do veiculo",
            Description = "Categoria a que o veiculo pertence")]
        public int CategoriaId { get; set; }
        public CategoriaVeiculo Categoria { get; set; }
        
        [Display(Name = "Localização", Prompt = "Insira a localização do veiculo",
            Description = "Localização onde o veiculo se encontra")]
        public int LocalizacaoId { get; set; }
        public Localizacao Localizacao { get; set; }
        
        [Display(Name = "Empresa", Prompt = "Insira a empresa a que pertence veiculo",
            Description = "Empresa dona do veiculo")]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}
