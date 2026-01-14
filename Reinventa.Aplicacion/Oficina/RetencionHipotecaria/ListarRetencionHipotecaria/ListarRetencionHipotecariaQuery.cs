using Azure;
using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Reinventa.Aplicacion.Oficina.RetencionHipotecaria.ListarRetencionHipotecaria
{
    public class ListarRetencionHipotecariaQuery
    : IRequest<PagedResult<RetencionHipotecariaSolicitudDTO>>
    {
        public string Usuario { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
