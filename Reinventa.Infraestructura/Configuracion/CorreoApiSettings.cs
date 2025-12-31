using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Infraestructura.Configuracion
{
    public class CorreoApiSettings
    {
        public string TokenUrl { get; set; }
        public string ApiUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Remitente { get; set; }
    }
}
