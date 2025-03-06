namespace Payment.Application.Common.Interfaces;

/// <summary>
/// Сервис управления блокировкой пользователей после нескольких неудачных попыток входа.
/// </summary>
public interface ILockoutService
{
    /// <summary>
    /// Проверяет, заблокирован ли пользователь.
    /// </summary>
    /// <param name="username"> Логин пользователя </param>
    /// <returns> True, если пользователь заблокирован, иначе False </returns>
    bool IsLockedOut(string username);
    
    /// <summary>
    /// Регистрирует неудачную попытку входа.
    /// Если количество попыток превышает допустимое, пользователь блокируется.
    /// </summary>
    /// <param name="username"> Логин пользователя </param>
    void RegisterFailedAttempt(string username);

    /// <summary>
    /// Сбрасывает счетчик неудачных попыток входа и снимает блокировку.
    /// </summary>
    /// <param name="username"> Логин пользователя </param>
    void ResetFailedAttempts(string username);
}