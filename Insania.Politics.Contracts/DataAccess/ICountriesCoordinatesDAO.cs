using Insania.Politics.Entities;

namespace Insania.Politics.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данными координат стран
/// </summary>
public interface ICountriesCoordinatesDAO
{
    /// <summary>
    /// Метод получения координаты страны по идентификатору
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты страны</param>
    /// <returns cref="CountryCoordinate?">Координата страны</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<CountryCoordinate?> GetById(long? id);

    /// <summary>
    /// Метод получения списка координат стран
    /// </summary>
    /// <returns cref="List{CountryCoordinate}">Список координат стран</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<CountryCoordinate>> GetList();

    /// <summary>
    /// Метод получения списка координат стран по идентификатору страны
    /// </summary>
    /// <param cref="long?" name="countryId">Идентификатор страны</param>
    /// <returns cref="List{CountryCoordinate}">Список координат стран</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<CountryCoordinate>> GetList(long? countryId);

    /// <summary>
    /// Метод восстановления координаты страны
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты страны</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="bool">Признак успешности</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<bool> Restore(long? id, string username);

    /// <summary>
    /// Метод закрытия координаты страны
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты страны</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="bool">Признак успешности</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<bool> Close(long? id, string username);
}