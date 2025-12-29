using Reinventa.Dominio.NPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Utilitarios.DTOS
{
    public class EncuestaDto
    {
        public int IdEncuesta { get; set; }
        public string? NombreEncuesta { get; set; }
        public string? TipoPersona { get; set; }
        public string? Link { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool FlagLogin { get; set; }
        public bool FlagBase { get; set; }
        public bool FlagAnalisis { get; set; }
        public string? TipoEncuesta { get; set; }
        public string? Estado { get; set; }
        public int CantidadRespuestas { get; set; }
        public byte[]? ImagenLogin { get; set; }
        public string? TituloEncuesta { get; set; }
        public ICollection<ClienteEncuestaDto>? ClientesEncuesta { get; set; }
        public ICollection<PreguntaDto>? EncuestaPreguntas { get; set; }
        public ICollection<RespuestaDto>? Respuestas { get; set; }
        public ICollection<DetalleRespuestaDto>? DetalleRespuestas { get; set; }
    }
}
