using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Oficina.VentaDigital.RegistrarVentaDigital
{
    public class RegistrarVentaDigitalRequest
    {
        public List<int> ProductosSeleccionados { get; set; } = new();

        public string CorreoCliente { get; set; }
        public string UsuarioRegistro { get; set; }

        public string DocumentoCliente { get; set; }
        public string NombreCliente { get; set; }
        public string CodigoVendedor { get; set; }
        public string CodOficina { get; set; }
    }
}
