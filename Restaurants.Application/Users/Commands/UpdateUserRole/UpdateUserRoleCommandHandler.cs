// Restaurants.Application/Users/Commands/UpdateUserRole/UpdateUserRoleCommandHandler.cs
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Services;

namespace Restaurants.Application.Users.Commands.UpdateUserRole;
public class UpdateUserRoleCommandHandler(
    ILogger<UpdateUserRoleCommandHandler> logger,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager,
    ITransactionService transactionService) : IRequestHandler<UpdateUserRoleCommand>
{
    public async Task Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating user role: {@Request}", request);

        // Validar que el usuario exista
        var user = await userManager.FindByEmailAsync(request.UserEmail)
            ?? throw new NotFoundException(nameof(User), request.UserEmail);

        // Validar que los roles existan
        var oldRole = await roleManager.FindByNameAsync(request.OldRoleName)
            ?? throw new NotFoundException(nameof(IdentityRole), request.OldRoleName);

        var newRole = await roleManager.FindByNameAsync(request.NewRoleName)
            ?? throw new NotFoundException(nameof(IdentityRole), request.NewRoleName);

        // Validar que el usuario tenga el rol anterior
        if (!await userManager.IsInRoleAsync(user, oldRole.Name!))
        {
            throw new ValidationException($"El usuario {request.UserEmail} no tiene el rol {request.OldRoleName}");
        }

        // Validar que el nuevo rol sea diferente al anterior
        if (oldRole.Name == newRole.Name)
        {
            throw new ValidationException($"El nuevo rol debe ser diferente al rol actual");
        }

        // Actualizar el rol (remover el anterior y agregar el nuevo)
        await using var transaction = await transactionService.BeginTransactionAsync(cancellationToken);
        try
        {
            var removeResult = await userManager.RemoveFromRoleAsync(user, oldRole.Name!);
            if (!removeResult.Succeeded)
            {
                throw new ValidationException($"No se pudo remover el rol {oldRole.Name} del usuario {user.Email}");
            }

            var addResult = await userManager.AddToRoleAsync(user, newRole.Name!);
            if (!addResult.Succeeded)
            {
                throw new ValidationException($"No se pudo agregar el rol {newRole.Name} al usuario {user.Email}");
            }

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}