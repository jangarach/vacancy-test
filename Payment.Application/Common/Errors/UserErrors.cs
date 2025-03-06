using Payment.Application.Common.Models;

namespace Payment.Application.Common.Errors;

public static class UserErrors
{
    public static Error Unauthorized() => Error.Unauthorized("Не верный логин или пароль!");

    public static Error HasLoginDuplicate() => Error.BadRequest("Пользователь с таким логином уже существует!");

    public static Error TooManyRequests() => Error.TooManyRequests("Слишком много неудачных попыток. Попробуйте позже!");
}