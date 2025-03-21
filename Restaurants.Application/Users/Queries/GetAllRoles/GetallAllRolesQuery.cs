using MediatR;

namespace Restaurants.Application.Users.Queries.GetAllRoles;

public class GetAllRolesQuery : IRequest<IEnumerable<string>>
{
}