using Fitness.Application.Requests.Commands;
using Fitness.Application.Requests.Queries;
using Fitness.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.WebUI.Controllers
{
    [ApiController]
    [Route("api/requests")]
    public class RequestController : ControllerBase
    {
        private readonly ISender _sender;

        public RequestController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Authorize(Roles = "Athlete")]
        public async Task<IActionResult> Create(CreateRequest command)
        {
            await _sender.Send(command);
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Athlete")]
        [Route("{Id:int}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteRequest command)
        {
            await _sender.Send(command);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Athlete")]
        public async Task<IActionResult> Update(UpdateRequest command)
        {
            await _sender.Send(command);
            return Ok();
        }

        [HttpPatch]
        [Authorize(Roles = "Coach")]
        [Route("{Id:int}")]
        public async Task<IActionResult> Accept([FromRoute] AcceptRequest command)
        {
            await _sender.Send(command);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("me")]
        public async Task<ActionResult<List<RequestDto>?>> Get([FromQuery] GetCurrentUserRequests query)
        {
            return Ok(await _sender.Send(query));
        }
    }
}
