using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommandHandler(ILogger<AssignUserRoleCommandHandler> logger,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager) : IRequestHandler<AssignUserRoleCommand>
{
    public async Task Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        // Validar que el usuario exista
        var user = await userManager.FindByEmailAsync(request.UserEmail)
            ?? throw new NotFoundException(nameof(User), request.UserEmail);

        // Validar que el rol exista
        var role = await roleManager.FindByNameAsync(request.RoleName)
            ?? throw new NotFoundException(nameof(IdentityRole), request.RoleName);

        // Validar que el usuario no tenga ya asignado algún rol
        var userRoles = await userManager.GetRolesAsync(user);
        if (userRoles.Any())
        {
            throw new ValidationException($"El usuario {request.UserEmail} ya tiene asignado el rol: {string.Join(", ", userRoles)}");
        }

        var result = await userManager.AddToRoleAsync(user, role.Name!);
        if (!result.Succeeded)
        {
            throw new ValidationException($"No se pudo asignar el rol {role.Name} al usuario {user.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

    }
}