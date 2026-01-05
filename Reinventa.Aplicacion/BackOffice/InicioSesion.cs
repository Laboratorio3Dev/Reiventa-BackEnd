using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Reinventa.Aplicacion.Contratos;
using Reinventa.Dominio.BackOffice;
using Reinventa.Persistencia.Aprendizaje;
using Reinventa.Persistencia.BackOffice;
using Reinventa.Utilitarios;
using Reinventa.Utilitarios.DTOS;
using System.Data;

namespace Reinventa.Aplicacion.BackOffice
{
    public class InicioSesion
    {
        public class LoginUsuario
        {
            public class LoginUnico : IRequest<Login>
            {
                public string? Usuario { get; set; }
                public string? Password { get; set; }
            }

            public class Manejador : IRequestHandler<LoginUnico, Login>
            {
                private readonly LAB_Context _context;
                private readonly IJwtGenerador _jwtGenerador;

                public Manejador(LAB_Context context, IJwtGenerador jwtGenerador)
                {
                    _context = context;
                    _jwtGenerador = jwtGenerador;
                }

                public async Task<Login?> Handle(LoginUnico request, CancellationToken cancellationToken)
                {
                    var usuario = await _context.LAB_Usuario
                        .FirstOrDefaultAsync(x =>
                            x.UsuarioWindows == request.Usuario &&
                           // x.Password == CryptoHelper.Encrypt(request.Password) &&
                            x.Estado == 1, cancellationToken);

                    if (usuario == null)
                    {
                        return new Login
                        {
                            ErrorMessage = "Usuario o Contraseña incorrecto",
                            IsSuccess = false,
                            IdUsuario = 0,
                            IdOficina = 0,
                            NombreCompleto = string.Empty,
                            Token = string.Empty,
                            UsuarioWindows = string.Empty,
                            Correo = string.Empty,
                            PaginaPrincipal = string.Empty
                        };
                    }

                    return new Login
                    {
                        IsSuccess = true,
                        IdUsuario = usuario.IdUsuario,
                        IdOficina = usuario.IdOficina==null?0:(int) usuario.IdOficina,
                        NombreCompleto = usuario.NombreCompleto,
                        Token = _jwtGenerador.CrearToken(usuario),
                        UsuarioWindows = usuario.UsuarioWindows,
                        Correo = usuario.Correo,
                        PaginaPrincipal = usuario.PaginaPrincipal,
                        ErrorMessage = "OK",
                        MenuUsuario = await ObtenerMenuUsuario(usuario.UsuarioWindows),
                        RolesUsuario = await ObtenerRolesUsuario(usuario.UsuarioWindows)
                    };
                }

                private async Task<List<Menu>> ObtenerMenuUsuario(string? usuarioWindows)
                {
                    var param = new SqlParameter("@USUARIO", usuarioWindows);

                    return await _context.Menus
                        .FromSqlRaw("EXEC USP_BACKOFFICELAB_MENU @USUARIO", param)
                        .ToListAsync();
                }

                private async Task<List<Rol>> ObtenerRolesUsuario(string? usuarioWindows)
                {
                    var param = new SqlParameter("@USUARIO", usuarioWindows);

                    return await _context.Roles
                        .FromSqlRaw("EXEC USP_BACKOFFICELAB_ROLES_USUARIO @USUARIO", param)
                        .ToListAsync();
                }
            }
        }

        public class CambiarPassword
        {
            public class Ejecuta : IRequest<ResponseTransacciones>
            {
                public string? Usuario { get; set; }
                public string Password_Old { get; set; }
                public string Password_New { get; set; }
            }

            public class Manejador : IRequestHandler<Ejecuta, ResponseTransacciones>
            {
                private readonly SA_Context _context;

                public Manejador(SA_Context context)
                {
                    _context = context;
                }

                public async Task<ResponseTransacciones> Handle(
                    Ejecuta request,
                    CancellationToken cancellationToken)
                {
                    var parametros = new[]
                    {
                        new SqlParameter("@USUARIO", SqlDbType.VarChar, 40)
                        {
                            Value = (object?)request.Usuario ?? DBNull.Value
                        },
                        new SqlParameter("@PASSWORD_OLD", SqlDbType.VarChar, 100)
                        {
                            Value = CryptoHelper.Encrypt(request.Password_Old)
                        },
                        new SqlParameter("@PASSWORD_NEW", SqlDbType.VarChar, 100)
                        {
                            Value = CryptoHelper.Encrypt(request.Password_New)
                        }
                    };

                    var resultado = _context
                        .Set<SpResultado>()
                        .FromSqlRaw(
                            "EXEC USP_LAB_ACTUALIZAR_PASSWORD @USUARIO, @PASSWORD_OLD, @PASSWORD_NEW",
                            parametros
                        )
                        .AsNoTracking()
                        .AsEnumerable()     // 👈 rompe composición
                        .FirstOrDefault();

                    return new ResponseTransacciones
                    {
                        Message = resultado?.RESULT,
                        IsSuccess = resultado?.RESULT == "OK",
                        IdValue = 1
                    };
                }


            }


        }

    }
}
