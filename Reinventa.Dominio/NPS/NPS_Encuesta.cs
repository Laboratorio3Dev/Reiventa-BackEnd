using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.NPS
{
    public class NPS_Encuesta
    {
        [Key]
        public int IdEncuesta { get; set; }
        public string? NombreEncuesta { get; set; }
        public string? TipoPersona { get; set; }
        public string? Link { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool FlagLogin { get; set; }
        public string? UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public byte[]? ImagenLogin { get; set; }        
        public bool FlagBase { get; set; }
        public bool FlagAnalisis { get; set; }
        public string? TituloEncuesta { get; set; }                
        public ICollection<NPS_ClienteEncuesta>? ClientesEncuesta { get; set; }
        public ICollection<NPS_EncuestaPregunta> EncuestaPreguntas { get; set; } = new List<NPS_EncuestaPregunta>();
        public ICollection<NPS_Respuesta>? Respuestas { get; set; }
        public ICollection<NPS_DetalleRespuesta>? DetalleRespuestas{ get; set; }
    }
}
