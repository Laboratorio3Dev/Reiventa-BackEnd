using Reinventa.Dominio.NPS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.UsuarioRoles
{
    public class LAB_Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        public string? NombreCompleto { get; set; }
        public string? UsuarioWindows { get; set; }
        public string? Password { get; set; }
        public string? Correo { get; set; }
        public int? Estado { get; set; }
        public int? IdOficina { get; set; }
        public string? PaginaPrincipal { get; set; }
    }
}
