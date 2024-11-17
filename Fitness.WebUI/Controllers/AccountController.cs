using Fitness.Application.Identity.Commands;
using Fitness.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.WebUI.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly ISender _sender;

        public AccountController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [Route("refresh-token")]
        public async Task<ActionResult<string>> RefreshExpiredToken(RefreshExpiredToken command)
        {
            return Ok(await _sender.Send(command));
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<TokensDto>> RegisterNewUser(RegisterNewUser command)
        {
            return Ok(await _sender.Send(command));
        }

        [HttpPost]
        [Route("sign-in")]
        public async Task<ActionResult<TokensDto>> SignIn(SignIn command)
        {
            return Ok(await _sender.Send(command));
        }
    }
}
