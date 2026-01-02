using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.BackOffice
{
    public class Lab_InicioSesion
    {

        [Key]
        public int id { get; set; }
        public string? usuario { get; set; }
        public string? pass { get; set; }
        public string passEncriptada { get; set; }
        public DateTime fecha { get; set; }

    }
}
