using Insania.Politics.Entities;

namespace Insania.Politics.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным координат
/// </summary>
public interface ICoordinatesDAO
{
    /// <summary>
    /// Метод получения координаты по идентификатору
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты</param>
    /// <returns cref="CoordinatePolitics?">Координата</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<CoordinatePolitics?> GetById(long? id);

    /// <summary>
    /// Метод получения списка координат
    /// </summary>
    /// <returns cref="List{CoordinatePolitics}">Список координат</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<CoordinatePolitics>> GetList();

    /// <summary>
    /// Метод восстановления координаты
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="bool">Признак успешности</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<bool> Restore(long? id, string username);

    /// <summary>
    /// Метод закрытия координаты
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="bool">Признак успешности</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<bool> Close(long? id, string username);
}