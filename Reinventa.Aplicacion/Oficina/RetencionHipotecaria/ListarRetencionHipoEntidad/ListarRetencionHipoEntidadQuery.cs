using MediatR;
using Reinventa.Aplicacion.Oficina.RetencionHipotecaria.ListarRetencionHipotecariaProducto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Oficina.RetencionHipotecaria.ListarRetencionHipoEntidad
{
  
    public record ListarRetencionHipoEntidadQuery : IRequest<List<RetencionHipoEntidadDTO>>;
}
