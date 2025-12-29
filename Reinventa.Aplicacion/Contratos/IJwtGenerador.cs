
using Reinventa.Dominio.UsuarioRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Contratos
{
    public interface IJwtGenerador
    {
        string CrearToken(LAB_Usuario usuario);

        string CrearTokenLanding(string dato);
    }
}
