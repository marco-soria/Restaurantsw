using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Users.Queries.GetAllUserRoles;

public class GetAllUserRolesQueryHandler(
    UserManager<User> userManager) : IRequestHandler<GetAllUserRolesQuery, IEnumerable<UserRolesDto>>
{
    public async Task<IEnumerable<UserRolesDto>> Handle(GetAllUserRolesQuery request, CancellationToken cancellationToken)
    {
        var users = await userManager.Users.ToListAsync(cancellationToken);
        var result = new List<UserRolesDto>();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            result.Add(new UserRolesDto
            {
                UserEmail = user.Email!,
                Roles = roles
            });
        }

        return result;
    }
}