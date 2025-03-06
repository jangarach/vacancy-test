using Microsoft.Extensions.Caching.Memory;
using Payment.Application.Common.Interfaces;

namespace Payment.Application.Common.Services;

/// <inheritdoc />
public class LockoutService(IMemoryCache cache) : ILockoutService
{
    /// <summary>
    /// Максимальное количество неудачных попыток входа перед блокировкой.
    /// </summary>
    private const int MaxFailedAttempts = 5;
    
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(1);

    /// <inheritdoc />
    public bool IsLockedOut(string username)
    {
        return cache.TryGetValue($"lockout_{username}", out _);
    }

    /// <inheritdoc />
    public void RegisterFailedAttempt(string username)
    {
        var key = $"failed_attempts_{username}";

        // Получаем текущее количество неудачных попыток (если нет, начинаем с 0)
        if (!cache.TryGetValue<int>(key, out var attempts))
        {
            attempts = 0;
        }

        attempts++;

        // Сохраняем обновленное количество попыток входа в кэше на 15 минут
        cache.Set(key, attempts, TimeSpan.FromMinutes(15));

        // Если количество попыток превысило лимит, блокируем пользователя
        if (attempts >= MaxFailedAttempts)
        {
            cache.Set($"lockout_{username}", true, LockoutDuration);
        }
    }

    /// <inheritdoc />
    public void ResetFailedAttempts(string username)
    {
        cache.Remove($"failed_attempts_{username}");
        cache.Remove($"lockout_{username}");
    }
}