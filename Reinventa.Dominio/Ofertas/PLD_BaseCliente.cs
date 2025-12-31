
using System.ComponentModel.DataAnnotations;


namespace Reinventa.Dominio.Ofertas
{
    public class PLD_BaseCliente
    {
        [Key]
        public int Id { get; set; }
        public string? DOCUMENTO { get; set; }
        public string OF_S_PLDD_12 { get; set; }
        public string OF_S_PLDD_18 { get; set; }
        public string OF_S_PLDD_24 { get; set; }
        public string OF_S_PLDD_36 { get; set; }
        public string TASA_S_PLDD_12_FIN { get; set; }
        public string TASA_S_PLDD_18_FIN { get; set; }
        public string TASA_S_PLDD_24_FIN { get; set; }
        public string TASA_S_PLDD_36_FIN { get; set; }
             
    }
}
