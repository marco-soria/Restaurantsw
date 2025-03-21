using MediatR;

namespace Restaurants.Application.Users.Queries.GetAllUserRoles;

public class GetAllUserRolesQuery : IRequest<IEnumerable<UserRolesDto>>
{
}

public class UserRolesDto
{
    public string UserEmail { get; set; } = default!;
    public IEnumerable<string> Roles { get; set; } = default!;
}