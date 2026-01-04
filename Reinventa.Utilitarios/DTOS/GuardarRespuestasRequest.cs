using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Utilitarios.DTOS
{
    public class GuardarRespuestasRequest
    {

        public string EncuestaToken { get; set; } = "";
        public string UsuarioToken { get; set; } = "";
        public List<DetalleRespuestaDto> Detalles { get; set; } = new();
    }
}
