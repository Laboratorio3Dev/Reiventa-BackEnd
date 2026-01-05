using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.BackOffice
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string UsuarioWindows { get; set; }
        public string? Oficina { get; set; }
        public string Rol { get; set; }
        public string? Zona { get; set; }
        public int Estado { get; set; }
    }
}
