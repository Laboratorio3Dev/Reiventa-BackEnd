using MediatR;
using Reinventa.Aplicacion.Oficina.Productos.ListarProductos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Oficina.Productos.ObtenerProducto
{
    public class ObtenerProductoQuery : IRequest<ProductoDto>
    {
        public int IdProducto { get; set; }

        public ObtenerProductoQuery(int idProducto)
        {
            IdProducto = idProducto;
        }
    }
}