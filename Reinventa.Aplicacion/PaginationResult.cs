using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion
{
    public class PaginationResult
    {
        //// La lista de datos (la parte "genérica")
        //public IEnumerable<T> Items { get; set; } = new List<T>();

        //// Metadatos de paginación
        //public int TotalRegistros { get; set; }
        //public int PaginaActual { get; set; }
        //public int RegistrosPorPagina { get; set; }

        //// Propiedad calculada útil para el Frontend
        //public int TotalPaginas => (int)Math.Ceiling((double)TotalRegistros / RegistrosPorPagina);

        //// Útil para saber si hay más datos sin que el front haga cálculos
        //public bool TienePaginaAnterior => PaginaActual > 1;
        //public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;
    }
}
