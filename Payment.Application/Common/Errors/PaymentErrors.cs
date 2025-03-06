using Payment.Application.Common.Models;

namespace Payment.Application.Common.Errors;

public static class PaymentErrors
{
    public static Error NotFoundUser(int userId) => Error.NotFound($"Пользователь с идентификатором - {userId}");

    public static Error InsufficientFunds(decimal amount) =>
        Error.BadRequest($"Недостаточно средств: попытка списания {amount} usd, привело к отрицательному балансу. Операция не может быть выполнена.");

    public static Error InternalServerError(string message) => Error.InternalServerError(message);
}