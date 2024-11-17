using Fitness.Application.Exercises.Commands;
using Fitness.Application.Exercises.Queries;
using Fitness.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.WebUI.Controllers
{
    [ApiController]
    [Route("api/exercises")]
    public class ExerciseController : ControllerBase
    {
        private readonly ISender _sender;

        public ExerciseController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> Create(CreateExercise command)
        {
            await _sender.Send(command);
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> Delete(DeleteExercise command)
        {
            await _sender.Send(command);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> Update(UpdateExercise command)
        {
            await _sender.Send(command);
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles ="Coach")]
        public async Task<ActionResult<List<ExerciseDto>?>> Get([FromQuery] GetExercises query)
        {
            return Ok(await _sender.Send(query));
        }
    }
}
