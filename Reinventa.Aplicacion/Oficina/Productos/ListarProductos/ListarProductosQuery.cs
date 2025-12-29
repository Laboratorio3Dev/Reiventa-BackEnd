using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Oficina.Productos.ListarProductos
{
    public record ListarProductosQuery(string? TextoBusqueda) : IRequest<List<ProductoDto>>;
}
