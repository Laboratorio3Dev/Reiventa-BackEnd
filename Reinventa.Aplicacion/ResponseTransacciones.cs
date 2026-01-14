using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion
{
    public class ResponseTransacciones
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public int IdValue { get; set; }
        public object? Data { get; set; }
    }
}
