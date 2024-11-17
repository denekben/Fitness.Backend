using Fitness.Application.Users.Queries;
using Fitness.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.WebUI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController: ControllerBase
    {
        private readonly ISender _sender;
        public UserController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [Route("coaches")]
        public async Task<ActionResult<List<ProfileDto>?>> GetCoaches([FromQuery] GetCoaches query)
        {
            return Ok(await _sender.Send(query));
        }

        [HttpGet]
        [Authorize]
        [Route("me")]
        public async Task<ActionResult<ProfileDto?>> GetCurrentUser([FromQuery] GetCurrentUserInfo query)
        {
            return Ok(await _sender.Send(query));
        }
    }
}
