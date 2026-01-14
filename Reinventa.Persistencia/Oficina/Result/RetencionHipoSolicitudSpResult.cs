using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Persistencia.Oficina.Result
{
    [Keyless] // 👈 MUY IMPORTANTE
    public class RetencionHipoSolicitudSpResult
    {
        public int ID { get; set; }
        public string? PRODUCTO { get; set; }
        public string? DNI_CLIENTE { get; set; }
        public string? MONEDA { get; set; }
        public decimal? TASA_SOLICITADA { get; set; }
        public decimal? SALDO_CREDITO { get; set; }
        public decimal? TASA_OFRECIDA { get; set; }
        public string? ENTIDAD { get; set; }
        public string? ID_USUARIO { get; set; }
        public string? ID_GERENTE { get; set; }
        public string? DNI_GERENTE { get; set; }
        public int? ESTADO { get; set; }
        public DateTime? FECHA_REGISTRO { get; set; }
        public decimal? TASA_RESPUESTA { get; set; }
    }
}
