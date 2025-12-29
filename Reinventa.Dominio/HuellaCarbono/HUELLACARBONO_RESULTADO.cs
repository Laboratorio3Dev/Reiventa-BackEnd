using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.HuellaCarbono
{
    public class HUELLACARBONO_RESULTADO
    {
        [Key]
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public decimal TotalCeo { get; set; }
        public decimal Arboles { get; set; }
        public decimal Alcance1_Valor { get; set; }
        public decimal Alcance1_Porcentaje { get; set; }
        public decimal Alcance2_Valor { get; set; }
        public decimal Alcance2_Porcentaje { get; set; }
        public decimal Alcance3_Valor { get; set; }
        public decimal Alcance3_Porcentaje { get; set; }
    }
}
