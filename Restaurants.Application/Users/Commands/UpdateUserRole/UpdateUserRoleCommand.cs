using MediatR;

namespace Restaurants.Application.Users.Commands.UpdateUserRole;

public class UpdateUserRoleCommand : IRequest
{
    public string UserEmail { get; set; } = default!;
    public string OldRoleName { get; set; } = default!;
    public string NewRoleName { get; set; } = default!;
}