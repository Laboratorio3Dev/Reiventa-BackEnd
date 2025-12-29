using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.NPS
{
    public class NPS_DetalleRespuesta
    {
        [Key]
        public int IdDetalleRespuesta { get; set; }

        public int? IdRespuesta { get; set; }

        public int? IdPregunta { get; set; }

        public string? ValorRespuesta { get; set; }
        public string? ValorComentario { get; set; }
        public int? Abandono { get; set; }
        public string? Emocion { get; set; }
        public string? CategoriaComentario { get; set; }
        public string? PalabraClave { get; set; }
        public string? TipoComentario { get; set; }

        [ForeignKey(nameof(IdRespuesta))]
        public NPS_Respuesta? Respuesta { get; set; }

        [ForeignKey(nameof(IdPregunta))]
        public NPS_EncuestaPregunta? Pregunta { get; set; }
    }

}
