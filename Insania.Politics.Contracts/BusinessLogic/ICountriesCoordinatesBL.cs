using Insania.Politics.Models.Responses.CountriesCoordinates;

namespace Insania.Politics.Contracts.BusinessLogic;

/// <summary>
/// Интерфейс работы с бизнес-логикой координат стран
/// </summary>
public interface ICountriesCoordinatesBL
{
    /// <summary>
    /// Метод получения списка координат стран по идентификатору страны
    /// </summary>
    /// <param cref="long?" name="countryId">Идентификатор страны</param>
    /// <returns cref="CountriesCoordinatesResponseList">Список координат стран</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<CountriesCoordinatesResponseList> GetList(long? countryId);
}