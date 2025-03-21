using MediatR;
using Microsoft.AspNetCore.Identity;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Queries.GetUserRoles;

public class GetUserRolesQueryHandler(
    UserManager<User> userManager) : IRequestHandler<GetUserRolesQuery, IEnumerable<string>>
{
    public async Task<IEnumerable<string>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.UserEmail)
            ?? throw new NotFoundException(nameof(User), request.UserEmail);

        return await userManager.GetRolesAsync(user);
    }
}