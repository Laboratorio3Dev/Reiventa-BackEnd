using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Oficina.VentaDigital.RegistrarVentaDigital
{
    public class RegistrarVentaDigitalCommand : IRequest<ResponseTransacciones>
    {
        public RegistrarVentaDigitalRequest Request { get; set; }
    }
}
