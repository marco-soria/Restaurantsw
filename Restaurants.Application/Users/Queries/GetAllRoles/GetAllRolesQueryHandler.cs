using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Restaurants.Application.Users.Queries.GetAllRoles;

public class GetAllRolesQueryHandler(
    RoleManager<IdentityRole> roleManager) : IRequestHandler<GetAllRolesQuery, IEnumerable<string>>
{
    public async Task<IEnumerable<string>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        return await roleManager.Roles
            .Select(r => r.Name!)
            .ToListAsync(cancellationToken);
    }
}