using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.Aprendizaje
{
    public class SA_DASHBOARD
    {
        [Key]
        public int ID { get; set; }

        public int ANIO { get; set; }

        public int MES { get; set; }

        [StringLength(40)]
        public string? USUARIO { get; set; }

        public int ID_PRODUCTO { get; set; }

        public int ID_DIMENSION { get; set; }

        [StringLength(200)]
        public string? INDICADOR { get; set; }

        [StringLength(200)]
        public string? UMBRAL { get; set; }

        [StringLength(200)]
        public string? RESULTADO { get; set; }

        public int CUMPLIMIENTO { get; set; }

        public DateTime FECHA_CARGA { get; set; }

        [StringLength(40)]
        public string? USUARIO_CARGA { get; set; }
    }
}
