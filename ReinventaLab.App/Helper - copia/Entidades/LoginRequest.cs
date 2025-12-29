using System.ComponentModel.DataAnnotations;

namespace ReinventaLab.App.Helper.Entidades
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; set; }
    }
}
