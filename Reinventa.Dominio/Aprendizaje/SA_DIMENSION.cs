using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.Aprendizaje
{
    public class SA_DIMENSION
    {
        [Key]
        public int Id_Dimension { get; set; }
        public string? Dimension { get; set; }
        public int? Estado { get; set; }
    }
}
