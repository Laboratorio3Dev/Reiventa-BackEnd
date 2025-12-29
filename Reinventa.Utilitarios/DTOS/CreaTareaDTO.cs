using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Utilitarios.DTOS
{
    public class CreaTareaDTO
    {
        public int Id_Tarea { get; set; }
        public int Usuario { get; set; }

        public DateTime FechaTarea { get; set; }
    }
}
