using MediatR;
using Reinventa.Aplicacion.Oficina.Productos.ListarProductos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Oficina.RetencionHipotecaria.ListarRetencionHipotecariaProducto
{
    public record ListarRetencionHipotecariaProductoQuery : IRequest<List<RetencionHipotecariaProductoDTO>>;
    
}
