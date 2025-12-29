using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.Aprendizaje
{
    public class SA_PRODUCTO
    {

        [Key]
        public int Id_Producto { get; set; }
        public string? Producto { get; set; }
        public int? Estado { get; set; }
    }
}
