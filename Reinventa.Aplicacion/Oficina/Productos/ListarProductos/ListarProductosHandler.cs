using MediatR;
using Microsoft.EntityFrameworkCore;
using Reinventa.Persistencia.NPS;
using Reinventa.Persistencia.Oficina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Oficina.Productos.ListarProductos
{
    public class ListarProductosHandler : IRequestHandler<ListarProductosQuery, List<ProductoDto>>
    {
        private readonly OFI_Context _context;

        public ListarProductosHandler(OFI_Context context)
        {
            _context = context;
        }

        public async Task<List<ProductoDto>> Handle(
            ListarProductosQuery request,
            CancellationToken cancellationToken)
        {
            var query = _context.OFI_Producto
           .AsNoTracking()
           .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.TextoBusqueda))
            {
                query = query.Where(p =>
                    p.Titulo.Contains(request.TextoBusqueda));
            }
          
           query = query.OrderBy(p => p.Orden);
            return await query
                .Select(p => new ProductoDto
                {
                    IdProducto = p.IdProducto,
                    Titulo = p.Titulo,
                    SubTitulo = p.SubTitulo,
                    Asunto = p.Asunto,
                    Orden = p.Orden,
                    Activo = p.Activo
                })
                .ToListAsync(cancellationToken);
        }
    }
}