using Reinventa.Dominio.NPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Reinventa.Utilitarios.DTOS
{
    public class RespuestaDto
    {
        public int IdRespuesta { get; set; }
        public int? IdEncuesta { get; set; }
        public string NroDocumentoCliente { get; set; }
        public DateTime? Fecha { get; set; }
        public string? CodigoLogAsociado { get; set; }
        public string? LinkInicio { get; set; }
        public ICollection<DetalleRespuestaDto> DetallesRespuesta { get; set; } = new List<DetalleRespuestaDto>();
    }
}
