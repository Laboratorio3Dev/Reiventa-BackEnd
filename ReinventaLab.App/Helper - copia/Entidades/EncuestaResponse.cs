namespace ReinventaLab.App.Helper.Entidades
{
    public class EncuestaResponse
    {
        public int IdEncuesta { get; set; }
        public string? NombreEncuesta { get; set; }
        public string? TipoPersona { get; set; }
        public string? Link { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int? FlagLogin { get; set; }
        public string? UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string? ImagenQR { get; set; }
        public int? FlagBase { get; set; }
        public int? FlagAnalisis { get; set; }
        public ICollection<EncuestaPreguntaResponse>? EncuestaPreguntas { get; set; }
        
    }
}
