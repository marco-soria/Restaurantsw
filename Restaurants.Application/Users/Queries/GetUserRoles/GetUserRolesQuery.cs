using MediatR;

namespace Restaurants.Application.Users.Queries.GetUserRoles;

public class GetUserRolesQuery : IRequest<IEnumerable<string>>
{
    public string UserEmail { get; set; } = default!;
}