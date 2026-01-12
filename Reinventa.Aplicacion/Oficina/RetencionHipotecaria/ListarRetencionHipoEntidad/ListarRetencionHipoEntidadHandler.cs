using MediatR;
using Microsoft.EntityFrameworkCore;
using Reinventa.Aplicacion.Oficina.RetencionHipotecaria.ListarRetencionHipotecariaProducto;
using Reinventa.Persistencia.Oficina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Oficina.RetencionHipotecaria.ListarRetencionHipoEntidad
{
    internal class ListarRetencionHipoEntidadHandler : IRequestHandler<ListarRetencionHipoEntidadQuery, List<RetencionHipoEntidadDTO>>
    {
        private readonly OFI_Context _context;

        public ListarRetencionHipoEntidadHandler(OFI_Context context)
        {
            _context = context;
        }

        public async Task<List<RetencionHipoEntidadDTO>> Handle(
            ListarRetencionHipoEntidadQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.OFI_RetencionHipoEntidad
            .Where(x => x.ESTADO == 1)
            .OrderBy(x => x.ENTIDAD)
            .Select(x => new RetencionHipoEntidadDTO
            {
                ID = x.ID,
                ENTIDAD = x.ENTIDAD
            })
            .ToListAsync(cancellationToken);
        }
    }
}