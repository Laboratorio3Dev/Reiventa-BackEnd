using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Utilitarios
{
    public static class FechaHelper
    {
        public static string NombreMes(int mes)
        {
            if (mes < 1 || mes > 12)
                return string.Empty;

            return CultureInfo
                .GetCultureInfo("es-PE")
                .DateTimeFormat
                .GetMonthName(mes);
        }
    }

}
