using MediatR;
using Microsoft.EntityFrameworkCore;
using Reinventa.Aplicacion.Oficina.Productos.ListarProductos;
using Reinventa.Persistencia.Oficina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Oficina.RetencionHipotecaria.ListarRetencionHipotecariaProducto
{
    public class ListarRetencionHipotecariaProductoHandler : IRequestHandler<ListarRetencionHipotecariaProductoQuery, List<RetencionHipotecariaProductoDTO>>
    {
        private readonly OFI_Context _context;

        public ListarRetencionHipotecariaProductoHandler(OFI_Context context)
        {
            _context = context;
        }

        public async Task<List<RetencionHipotecariaProductoDTO>> Handle(
            ListarRetencionHipotecariaProductoQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.OFI_RetencionHipoProductos
            .Where(x => x.ESTADO == 1)
            .OrderBy(x => x.PRODUCTO)
            .Select(x => new RetencionHipotecariaProductoDTO
            {
                ID = x.ID,
                PRODUCTO = x.PRODUCTO
            })
            .ToListAsync(cancellationToken);
        }
    }
}