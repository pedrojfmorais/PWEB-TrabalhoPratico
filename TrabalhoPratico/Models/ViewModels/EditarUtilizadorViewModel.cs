using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TrabalhoPratico.Models.ViewModels
{
    public class EditarUtilizadorViewModel
    {
        public ApplicationUser utilizador;
        public List<RolesViewModel> roles;
    }
}
