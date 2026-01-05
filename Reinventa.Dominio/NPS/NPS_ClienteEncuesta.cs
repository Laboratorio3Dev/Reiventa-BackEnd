using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Reinventa.Dominio.NPS
{
    public class NPS_ClienteEncuesta
    {
        [Key]
        public int IdCliente { get; set; }
        public int IdEncuesta { get; set; }
        public string? NroDocumento { get; set; }
        public string? CodigoIBS { get; set; }
        public string? Nombre { get; set; }
        public string? Correo { get; set; }
        public string? Celular { get; set; }
        public int? FlagContesta { get; set; }
        public string? LinkPersonalizado { get; set; }
        public string? UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }

        [JsonIgnore]
        public NPS_Encuesta Encuesta { get; set; } = null!;
        
    }
}
