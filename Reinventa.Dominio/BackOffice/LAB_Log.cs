using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.BackOffice
{
    public class LAB_Log
    {
        [Key]
        public int Id_Log { get; set; }
        public string? Codigo_Unico { get; set; }
        public int? Paso { get; set; }
        public string? Detalle_Log { get; set; }
        public DateTime? Fecha_Log { get; set; }
        public string? Ip_Cliente { get; set; }
        public string? Utm { get; set; }
        public string? Source { get; set; }
        public string? Medium { get; set; }
        public string? Campaign { get; set; }
        public string? Sistema_Operativo { get; set; }
    }
}
