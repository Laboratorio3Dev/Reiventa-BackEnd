using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.HuellaCarbono
{
    public class HUELLACARBONO_SOLICITUDCLIENTE
    {
        [Key]
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public string? Ciiu { get; set; }
        public string? NombreContacto { get; set; }
        public string? PuestoContacto { get; set; }
        public string? CorreoContacto { get; set; }
        public string? TelefonoContacto { get; set; }
        public decimal? DieselEquipo { get; set; }
        public decimal? GasolinaEquipo { get; set; }
        public decimal? GlpEquipo { get; set; }
        public decimal? GnvEquipo { get; set; }
        public decimal? DieselVehiculo { get; set; }
        public decimal? GasolinaVehiculo { get; set; }
        public decimal? GlpVehiculo { get; set; }
        public decimal? GnvVehiculo { get; set; }
        public decimal? Electricidad { get; set; }
        public decimal? Agua { get; set; }
        public decimal? HojasA4 { get; set; }
        public decimal? HojasA3 { get; set; }
        public decimal? Plasticos { get; set; }
        public decimal? PapelCarton { get; set; }
        public decimal? Residuos { get; set; }
        public decimal? Metal { get; set; }
        public decimal? Raee { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
