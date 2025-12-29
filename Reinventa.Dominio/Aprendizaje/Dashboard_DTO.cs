using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.Aprendizaje
{
    public class Dashboard_DTO
    {
        public string? EJECUTIVO { get; set; }
        public string? PRODUCTO { get; set; }
        public string? CODIGO { get; set; }
        public string? DIMENSION { get; set; }
        public string? INDICADOR { get; set; }
        public string? UMBRAL { get; set; }
        public string? RESULTADO { get; set; }
        public int? CUMPLIMIENTO { get; set; }
        public string? ZONA { get; set; }
        public string? OFICINA { get; set; }
        public int? ANIO { get; set; }
        public int? MES { get; set; }
    }
}
