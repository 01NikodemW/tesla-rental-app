using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workshop.Application.Reservations.Commands.CreateReservation;

namespace Workshop.API.Controllers;

[ApiController]
[Route("api/reservations")]
public class ReservationController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateReservation([FromBody] CreateReservationCommand command)
    {
        var userIdString = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(userIdString, out var userId))
        {
            command.UserId = userId;
            var id = await mediator.Send(command);
            return Ok(id);
        }

        return BadRequest("Invalid user ID");
    }
}