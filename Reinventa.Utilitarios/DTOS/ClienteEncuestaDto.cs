using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Utilitarios.DTOS
{
    public class ClienteEncuestaDto
    {
        public int IdCliente { get; set; }
        public int IdEncuesta { get; set; }
        public string Celular { get; set; }
        public string CodigoIBS { get; set; }
        public string Correo { get; set; }
        public int? FlagContesta { get; set; }
        public string Nombre { get; set; }
        public string NroDocumento { get; set; }
        public string LinkPersonalizado { get; set; }
        public string? UsuarioCreacion { get; set; }
    }

}
