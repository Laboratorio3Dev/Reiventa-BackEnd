using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Oficina.Productos.EliminarProducto
{
    public class EliminarProductoCommand : IRequest<ResponseTransacciones>
    {
        public int IdProducto { get; set; }
    }
}
