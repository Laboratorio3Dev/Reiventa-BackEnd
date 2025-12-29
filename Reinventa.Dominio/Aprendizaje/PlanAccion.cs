using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.Aprendizaje
{
    public class PlanAccion
    {
        public int ID_PLANACCION { get; set; }
        public string? PRODUCTO { get; set; }
        public string? DIMENSION { get; set; }
        public string? TAREA { get; set; }
        public string? USUARIO { get; set; }
        public string? FECHA { get; set; }
        public string? ESTADO { get; set; }
        public string? ComentarioEvidencia { get; set; }
        public string? Oficina { get; set; }
        public string? Zona { get; set; }
        public string? GerenteOficina { get; set; }
        public byte[]? Evidencia { get; set; }
    }
}
