using Azure;
using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Reinventa.Aplicacion.Oficina.RetencionHipotecaria.ListarRetencionHipotecaria
{
    public record ListarRetencionHipotecariaQuery(
    string Usuario,
    DateTime? FechaInicio,
    DateTime? FechaFin,
    int Page,
    int PageSize
) : IRequest<List<RetencionHipotecariaSolicitudDTO>>;
}
