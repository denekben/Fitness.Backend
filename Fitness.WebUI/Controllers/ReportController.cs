using Fitness.Application.Reports.Commands;
using Fitness.Application.Reports.Queries;
using Fitness.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.WebUI.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportController : ControllerBase
    {
        private readonly ISender _sender;

        public ReportController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> Create(CreateReport command)
        {
            await _sender.Send(command);
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Coach")]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteReport command)
        {
            await _sender.Send(command);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> Update(UpdateReport command)
        {
            await _sender.Send(command);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("me")]
        public async Task<ActionResult<List<ReportDto>?>> Get([FromQuery] GetReportsForCurrentUser query)
        {
            return Ok(await _sender.Send(query));
        }
    }
}
