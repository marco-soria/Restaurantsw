
using Restaurants.Domain.Exceptions;

namespace Restaurants.API.Middlewares;

public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NotFoundException notFound)
        {
            context.Response.StatusCode = 404;
            context.Response.ContentType = "application/json";
            var error = new { message = notFound.Message };
            await context.Response.WriteAsJsonAsync(error);
            logger.LogWarning(notFound.Message);
        }
        catch (ValidationException validationEx)
        {
            context.Response.StatusCode = 400; // Bad Request
            context.Response.ContentType = "application/json";
            var error = new { message = validationEx.Message };
            await context.Response.WriteAsJsonAsync(error);
            logger.LogWarning(validationEx.Message);
        }
        catch (ForbidException forbidden)
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";
            var error = new { message = forbidden.Message };
            await context.Response.WriteAsync("Access forbidden");
            logger.LogWarning(forbidden.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            // En producción, no devolver detalles del error interno
            var isDevelopment = context.RequestServices
                .GetRequiredService<IWebHostEnvironment>()
                .IsDevelopment();

            var error = new
            {
                message = isDevelopment ? ex.Message : "Something went wrong",
                stackTrace = isDevelopment ? ex.StackTrace : null
            };

            await context.Response.WriteAsJsonAsync(error);
        }
    }
}