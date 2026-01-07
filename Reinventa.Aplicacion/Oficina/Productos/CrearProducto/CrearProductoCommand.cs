using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Oficina.Productos.CrearProducto
{
    public class CrearProductoCommand : IRequest<ResponseTransacciones>
    {
        public string Titulo { get; set; }
        public string? SubTitulo { get; set; }
        public string Asunto { get; set; }
        public int? Orden { get; set; }
        public string FormatoCorreo { get; set; }
    }
}
