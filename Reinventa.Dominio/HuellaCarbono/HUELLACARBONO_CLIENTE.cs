using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.HuellaCarbono
{
    public class HUELLACARBONO_CLIENTE
    {
        [Key]
        public int Id { get; set; }
        public string? Ruc { get; set; }
        public string? RazonSocial { get; set; }
        public string? Clave { get; set; }
        public int Estado { get; set; }
        public int FlagCompletado { get; set; }
    }
}
