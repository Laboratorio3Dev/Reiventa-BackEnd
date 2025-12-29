using ReinventaLab.App.Pages.Shared;
using ReinventaLab.App.Helper.Entidades;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ReinventaLab.App.Helper.Entidades
{
    public static class IQueryableExtensions
    {
        public async static Task<int> CalcularTotalPaginas<T>(this IQueryable<T> queryable,
            int cantidadRegistrosAMostrar)
        {
            double conteo = await queryable.CountAsync();
            int totalPaginas = (int)Math.Ceiling(conteo / cantidadRegistrosAMostrar);
            return totalPaginas;
        }

        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, Paginacion.Paginacion paginacion)
        {
            return queryable
                .Skip((paginacion.Pagina - 1) * paginacion.CantidadRegistros)
                .Take(paginacion.CantidadRegistros);
        }

        public async static Task<int> CalcularTotalPaginasPedidos<T>(this IQueryable<T> queryable,
            int cantidadRegistrosAMostrar)
        {
            double conteo = await queryable.CountAsync();
            int totalPaginas = (int)Math.Ceiling(conteo / cantidadRegistrosAMostrar);
            return totalPaginas;
        }

       
    }
}
