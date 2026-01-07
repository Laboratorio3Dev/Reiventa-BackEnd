using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Oficina.CargarRetencion
{
    public record CargarRetencionExcelCommand(
     IFormFile Archivo
 ) : IRequest<ResponseTransacciones>;
}
