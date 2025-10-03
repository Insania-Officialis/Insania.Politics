using Insania.Politics.Entities;

namespace Insania.Politics.Contracts.Services;

/// <summary>
/// Интерфейс сервиса фонового логгирования в бд
/// </summary>
public interface ILoggingSL
{
    /// <summary>
    /// Метод постановки лога в очередь на обработку
    /// </summary>
    /// <param cref="LogApiPolitics" name="log">Лог для записи</param>
    /// <returns cref="ValueTask">Задание</returns>
    ValueTask QueueLogAsync(LogApiPolitics log);
}