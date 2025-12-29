using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.Aprendizaje
{
    public class SA_DASHBOARD_COMENTARIO
    {
        [Key]
        public int ID { get; set; }

        [StringLength(40)]
        public string? USUARIO { get; set; }
        public int ID_PRODUCTO { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string? RESUMEN { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string? INSIGHT { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string? PREGUNTAS { get; set; }
        public DateTime FECHA_CARGA { get; set; }

        [StringLength(40)]
        public string USUARIO_CARGA { get; set; }
        public int ANIO { get; set; }
        public int MES { get; set; }
    }
}
