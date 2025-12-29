using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reinventa.Aplicacion;
using Reinventa.Aplicacion.BackOffice;

namespace ReinventaLab.Api.Controllers.BackOffice
{
    //https://localhost:44396/api/BackOffice/Login
    [Route("api/BackOffice/[controller]")]
        [ApiController]
        [AllowAnonymous]
        public class LoginController : ControllerBase
        {
            private readonly IMediator _mediator;

            public LoginController(IMediator mediator)
            {
                _mediator = mediator;
            }

            [HttpPost("acceso")]
            public async Task<ActionResult<Login>> Login(InicioSesion.LoginUsuario.LoginUnico data)
            {
                return await _mediator.Send(data);
            }

            [HttpPost("CambiarPassword")]
            public async Task<ActionResult<ResponseTransacciones>> CambiarPassword(InicioSesion.CambiarPassword.Ejecuta data)
            {
                return await _mediator.Send(data);
            }
        }
    }
