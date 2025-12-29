using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Utilitarios.DTOS
{
    public class DatosGeneralesDTO
    {
        public ICollection<CombosGeneral>? ListaDimensiones { get; set; }
        public ICollection<CombosGeneral>? ListaProductos { get; set; }
        public ICollection<CombosGeneral>? ListaColaboradores { get; set; }
    }
}
