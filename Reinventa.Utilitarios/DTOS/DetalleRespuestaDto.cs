using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Utilitarios.DTOS
{
    public class DetalleRespuestaDto
    {
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
    }
}
