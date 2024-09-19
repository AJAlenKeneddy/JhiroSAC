using System.ComponentModel.DataAnnotations;

namespace JhiroServer.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        [EmailAddress(ErrorMessage = "Correo electrónico inválido.")]
        public string Correo { get; set; } = string.Empty;

        
    }
}
