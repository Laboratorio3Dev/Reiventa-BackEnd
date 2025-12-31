

using Reinventa.Dominio.BackOffice;
using Reinventa.Dominio.UsuarioRoles;

namespace Reinventa.Aplicacion.BackOffice
{
    public class Login
    {
        public bool IsSuccess { get; set; }
        public int IdUsuario { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Token { get; set; }
        public string? UsuarioWindows { get; set; }
        public string? Correo { get; set; }    
        public string? PaginaPrincipal { get; set; }
        public string? ErrorMessage { get; set; }
        public ICollection<Menu> MenuUsuario { get; set; } = new List<Menu>();
        public ICollection<Rol> RolesUsuario { get; set; } = new List<Rol>();
        public int IdOficina { get; set; }

    }
}
