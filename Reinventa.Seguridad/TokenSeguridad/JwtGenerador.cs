
using Microsoft.IdentityModel.Tokens;
using Reinventa.Aplicacion.Contratos;
using Reinventa.Dominio.UsuarioRoles;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Reinventa.Seguridad.TokenSeguridad
{
    public class JwtGenerador : IJwtGenerador
    {
        public string CrearToken(LAB_Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId , usuario.UsuarioWindows)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("LaboratorioInnovacion_BanBif_2025_AWS"));
            var credenciales= new SigningCredentials(key,SecurityAlgorithms.HmacSha256Signature);

            var tokenDescripcion = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                NotBefore= DateTime.Now,
                Expires = DateTime.Now.AddMinutes(22),
                SigningCredentials= credenciales,
            };
                      

            var tokenManejador = new JwtSecurityTokenHandler();
            var token = tokenManejador.CreateToken(tokenDescripcion);
            return tokenManejador.WriteToken(token).ToString();
        }

        public string CrearTokenLanding(string dato)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name , dato)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("LaboratorioInnovacion_BanBif_2025_AWS"));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescripcion = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = credenciales,
            };

            var tokenManejador = new JwtSecurityTokenHandler();
            var token = tokenManejador.CreateToken(tokenDescripcion);
            return tokenManejador.WriteToken(token).ToString();
        }
    }
}
