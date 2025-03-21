
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Users.Commands.AssignUserRole;
using Restaurants.Application.Users.Commands.UnassignUserRole;
using Restaurants.Application.Users.Commands.UpdateUserRole;
using Restaurants.Application.Users.Commands.UpdateUserDetails;
using Restaurants.Application.Users.Queries.GetAllRoles;
using Restaurants.Application.Users.Queries.GetAllUserRoles;
using Restaurants.Application.Users.Queries.GetUserRoles;
using Restaurants.Domain.Constants;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController(IMediator mediator) : ControllerBase
{
    [HttpPut("user")]
    [Authorize]
    public async Task<IActionResult> UpdateUserDetails(UpdateUserDetailsCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("userRole")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> AssignUserRole(AssignUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("userRole")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> UnassignUserRole(UnassignUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("userRole")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> UpdateUserRole(UpdateUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet("userRoles/{email}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> GetUserRoles(string email)
    {
        var result = await mediator.Send(new GetUserRolesQuery { UserEmail = email });
        return Ok(result);
    }

    [HttpGet("userRoles")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> GetAllUserRoles()
    {
        var result = await mediator.Send(new GetAllUserRolesQuery());
        return Ok(result);
    }

    [HttpGet("roles")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> GetAllRoles()
    {
        var result = await mediator.Send(new GetAllRolesQuery());
        return Ok(result);
    }
}