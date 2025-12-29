using Reinventa.Dominio.NPS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.UsuarioRoles
{
    public class LAB_Rol
    {
        [Key]
        public int IdRol { get; set; }
        public string? Rol { get; set; }
        public int? Estado { get; set; }
      //  public ICollection<LAB_Usuario> Usuarios { get; set; } = new List<LAB_Usuario>();
    }
}
