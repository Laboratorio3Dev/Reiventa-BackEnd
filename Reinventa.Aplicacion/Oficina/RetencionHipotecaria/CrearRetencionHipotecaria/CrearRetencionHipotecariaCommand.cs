using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Oficina.RetencionHipotecaria.CrearRetencionHipotecaria
{
    public class CrearRetencionHipotecariaCommand : IRequest<ResponseTransacciones>
    {
        public string NroPrestamo { get; set; } = null!;
        public decimal TasaSolicitada { get; set; }
        public string Moneda { get; set; } = null!;
        public decimal SaldoCredito { get; set; }
        public int EntidadId { get; set; }
        public decimal TasaOfrecida { get; set; }
        public int ProductoId { get; set; }
        public string Usuario { get; set; } = null!;
    }
}
