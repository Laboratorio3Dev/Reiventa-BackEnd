using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.NPS
{
    public class NPS_Respuesta
    {
        [Key]
        public int IdRespuesta { get; set; }
        public int? IdEncuesta { get; set; }
        public string? NroDocumentoCliente { get; set; }
        public DateTime? Fecha { get; set; }
        public string? CodigoLogAsociado { get; set; }
        public string? LinkInicio { get; set; }
        public string? IP_REGISTRO { get; set; }

        [ForeignKey(nameof(IdEncuesta))]
        public NPS_Encuesta? Encuesta { get; set; }
        public ICollection<NPS_DetalleRespuesta>? DetallesRespuesta { get; set; }
    }

}
