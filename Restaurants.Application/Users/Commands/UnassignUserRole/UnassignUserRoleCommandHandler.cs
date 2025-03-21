
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users.Commands.AssignUserRole;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.UnassignUserRole;

public class UnassignUserRoleCommandHandler(ILogger<UnassignUserRoleCommandHandler> logger,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager) : IRequestHandler<UnassignUserRoleCommand>
{
    public async Task Handle(UnassignUserRoleCommand request, CancellationToken cancellationToken)
    {
        // Validar que el usuario exista
        var user = await userManager.FindByEmailAsync(request.UserEmail)
            ?? throw new NotFoundException(nameof(User), request.UserEmail);

        // Validar que el rol exista
        var role = await roleManager.FindByNameAsync(request.RoleName)
            ?? throw new NotFoundException(nameof(IdentityRole), request.RoleName);

        // Validar que el usuario tenga el rol asignado
        if (!await userManager.IsInRoleAsync(user, role.Name!))
        {
            throw new ValidationException($"El usuario {request.UserEmail} no tiene asignado el rol {request.RoleName}");
        }

        var result = await userManager.RemoveFromRoleAsync(user, role.Name!);
        if (!result.Succeeded)
        {
            throw new ValidationException($"No se pudo remover el rol {role.Name} del usuario {user.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

    }
}