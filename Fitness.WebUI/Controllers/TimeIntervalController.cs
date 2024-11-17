using Fitness.Application.Shedules.Commands;
using Fitness.Application.Shedules.Queries;
using Fitness.Application.TimeIntervals.Queries;
using Fitness.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.WebUI.Controllers
{
    [ApiController]
    [Route("api/intervals")]
    public class TimeIntervalController : ControllerBase
    {
        private readonly ISender _sender;

        public TimeIntervalController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateTimeInterval command)
        {
            await _sender.Send(command);
            return Ok();
        }

        [HttpDelete]
        [Authorize]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteTimeInterval command)
        {
            await _sender.Send(command);
            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(UpdateTimeInterval command)
        {
            await _sender.Send(command);
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles ="Coach")]
        public async Task<ActionResult<List<TimeIntervalDto>?>> Get([FromQuery] GetTimeIntervals query)
        {
            return Ok(await _sender.Send(query));
        }

        [HttpGet]
        [Route("me")]
        [Authorize]
        public async Task<ActionResult<List<TimeIntervalDto>?>> Get([FromQuery] GetCurrentUserTimeIntervals query)
        {
            return Ok(await _sender.Send(query));
        }

    }
}
