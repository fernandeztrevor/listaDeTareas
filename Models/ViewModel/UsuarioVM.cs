using System.ComponentModel.DataAnnotations;

namespace ListaDeTareas.Models.ViewModel
{
    public class UsuarioVM
    {
        [Required(ErrorMessage = "Escriba su usuario")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Escriba la contraseña")]
        [MinLength(5, ErrorMessage = "Escriba al menos 5 caracteres")]
        [MaxLength(50, ErrorMessage = "Escriba un maximo de 50 caracteres")]
        public string Clave { get; set; }

    }
}