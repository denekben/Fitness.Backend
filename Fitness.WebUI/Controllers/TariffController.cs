using Fitness.Application.Tariffs.Commands;
using Fitness.Application.Tariffs.Queries;
using Fitness.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.WebUI.Controllers
{
    [ApiController]
    [Route("api/tariffs")]
    public class TariffController : ControllerBase
    {
        private readonly ISender _sender;

        public TariffController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Authorize(Roles ="Coach")]
        public async Task<IActionResult> Create(CreateTariff command)
        {
            await _sender.Send(command);
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Coach")]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteTariff command)
        {
            await _sender.Send(command);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> Update(UpdateTariff command)
        {
            await _sender.Send(command);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<TariffDto>?>> Get([FromQuery] GetTariff query)
        {
            return Ok(await _sender.Send(query));
        }
    }
}
