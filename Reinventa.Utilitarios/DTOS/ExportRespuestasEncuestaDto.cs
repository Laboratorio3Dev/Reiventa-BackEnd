using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Utilitarios.DTOS
{
    public class ExportRespuestasEncuestaDto
    {
        public List<ExportColumnDto> Columns { get; set; } = new();
        public List<Dictionary<string, string?>> Rows { get; set; } = new();
    }
}
