using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.NPS
{
    public class NPS_EncuestaPregunta
    {
        [Key]
        public int IdEncuestaPregunta { get; set; }
        public int? IdEncuesta { get; set; }
        public string? Pregunta { get; set; }
        public string? TipoPregunta { get; set; }
        public int? Orden { get; set; }
        public int? RangoMinimo { get; set; }
        public int? RangoMaximo { get; set; }
        public int? FlagComentario { get; set; }
        public string? Comentario { get; set; }
        public string? TextoDetractor { get; set; }
        public string? TextoNeutro { get; set; }
        public string? TextoPromotor { get; set; }
        public string? ValoresX { get; set; }
        public string? ValoresY { get; set; }
        public string? UsuarioCreacion { get; set; }
        public string? TextoValorMinimo { get; set; }
        public string? TextoValorMaximo { get; set; }        
        public DateTime? FechaCreacion { get; set; }
        public int? Estado { get; set; }

        [ForeignKey(nameof(IdEncuesta))]
        public NPS_Encuesta? Encuesta { get; set; }
    }
}
