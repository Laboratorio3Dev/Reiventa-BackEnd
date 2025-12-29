using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reinventa.Aplicacion;
using Reinventa.Aplicacion.BackOffice;
using Reinventa.Utilitarios.DTOS;

namespace ReinventaLab.Api.Controllers.BackOffice
{
    //https://localhost:44396/api/BackOffice/Log
    [Route("api/BackOffice/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LogAuditoriaController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LogAuditoriaController(IMediator mediator)
        {
            _mediator = mediator;
        }
       

        [HttpPost("CrearLog")]
        public async Task<ActionResult<ResponseTransacciones>> CrearLog(Log.CrearLog.Ejecuta data)
        {
            return await _mediator.Send(data);
        }
    }
}
