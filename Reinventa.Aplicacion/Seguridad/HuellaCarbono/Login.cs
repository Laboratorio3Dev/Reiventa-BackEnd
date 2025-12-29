using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Seguridad.HuellaCarbono
{
    public class Login
    {
        public int IdCliente{ get; set; }
        public bool IsSuccess { get; set; }
        public string? RazonSocial { get; set; }
        public string? Token { get; set; }
        public string? ErrorMessage { get; set; }
        public int FlagSolicitud { get; set; }
        public int FlagCompletado { get; set; }
    }
}
