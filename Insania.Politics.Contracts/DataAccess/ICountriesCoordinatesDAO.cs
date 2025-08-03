using Insania.Politics.Entities;

namespace Insania.Politics.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данными координат стран
/// </summary>
public interface ICountriesCoordinatesDAO
{
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
}