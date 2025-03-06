namespace Payment.Domain.Entities;

/// <summary>
/// Платежи
/// </summary>
public class UserPayment
{
    /// <summary>
    /// Внутренний идентификатор
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// Сумма
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Дата и время совершения платежа
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    /// <summary>
    /// Навигационное свойство
    /// </summary>
    public User User { get; set; } = null!;
}