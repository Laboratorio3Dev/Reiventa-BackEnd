using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reinventa.Aplicacion.HuellaCarbono;
using Reinventa.Aplicacion.Seguridad.HuellaCarbono;

namespace ReinventaLab.Api.Controllers.HuellaCarbono
{
    //https://localhost:44396/api/HuellaCarbono/Login
    [Route("api/HuellaCarbono/[controller]")]
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
        public async Task<ActionResult<Login>> Login(Consultas.LoginEmpresa.LoginUnico data)
        {
            return await _mediator.Send(data);
        }

    
    }
}
