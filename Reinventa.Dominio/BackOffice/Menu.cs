using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.BackOffice
{
    public class Menu
    {
        public int IdOpcion { get; set; }
        public string Opcion { get; set; }
        public string Url { get; set; }
        public int IdPadre { get; set; }
        public int Orden { get; set; }
        public string Icono { get; set; }
    }
}
