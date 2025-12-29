namespace ReinventaLab.App.Helper.Entidades
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public string? Token { get; set; }
        public int IdUsuario { get; set; }
        public string? UsuarioWindows { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Rol { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
