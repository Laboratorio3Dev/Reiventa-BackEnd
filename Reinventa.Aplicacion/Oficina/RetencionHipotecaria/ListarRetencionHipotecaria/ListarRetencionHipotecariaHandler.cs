using Aspose.Cells.Drawing;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reinventa.Persistencia.NPS;
using Reinventa.Persistencia.Oficina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iTextSharp.text.pdf.AcroFields;

namespace Reinventa.Aplicacion.Oficina.RetencionHipotecaria.ListarRetencionHipotecaria
{
    public class ListarRetencionHipotecariaHandler : IRequestHandler< ListarRetencionHipotecariaQuery, PagedResult<RetencionHipotecariaSolicitudDTO>>
    {
        private readonly NPS_Context _context;

        public ListarRetencionHipotecariaHandler(NPS_Context context)
        {
            _context = context;
        }


        public async Task<PagedResult<RetencionHipotecariaSolicitudDTO>> Handle(
    ListarRetencionHipotecariaQuery request,
    CancellationToken cancellationToken)
        {
            var data = await _context.Database
                .SqlQuery<RetencionHipotecariaSolicitudDTO>(@$"
            EXEC [dbo].[SP_RETENCION_HIPO_LISTAR_SOLICITUDES] 
            @USUARIO = {request.Usuario}, 
            @FECHA_INICIO = {request.FechaInicio}, 
            @FECHA_FIN = {request.FechaFin}")
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var total = data.Count;

            var items = data
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PagedResult<RetencionHipotecariaSolicitudDTO>
            {
                Items = items,
                Total = total,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
