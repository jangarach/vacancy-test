namespace Payment.Domain.Entities;

/// <summary>
/// Представляет пользователя системы
/// </summary>
public class User
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Логин
    /// </summary>
    public string Username { get; set; } = null!;
    
    /// <summary>
    /// Хэш пароля
    /// </summary>
    public string PasswordHash { get; set; } = null!;
    
    /// <summary>
    /// Баланс пользователя
    /// </summary>
    public decimal Balance { get; set; } = 8.0m;
    
    /// <summary>
    /// Список платежей пользователя
    /// </summary>
    public ICollection<UserPayment> Payments { get; set; } = new List<UserPayment>();
}