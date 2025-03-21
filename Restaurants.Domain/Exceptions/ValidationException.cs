// Restaurants.Domain/Exceptions/ValidationException.cs
namespace Restaurants.Domain.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message)
    {
    }
}