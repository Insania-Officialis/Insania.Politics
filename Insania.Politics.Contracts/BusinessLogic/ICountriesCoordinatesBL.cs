using Insania.Shared.Models.Responses.Base;

using Insania.Politics.Models.Requests.CountriesCoordinates;
using Insania.Politics.Models.Responses.CountryCoordinates;

namespace Insania.Politics.Contracts.BusinessLogic;

/// <summary>
/// Интерфейс работы с бизнес-логикой координат стран
/// </summary>
public interface ICountriesCoordinatesBL
{
    /// <summary>
    /// Метод получения списка координат страны по идентификатору страны
    /// </summary>
    /// <param cref="long?" name="countryId">Идентификатор страны</param>
    /// <returns cref="CountryCoordinatesResponseList">Список координат стран</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<CountryCoordinatesResponseList> GetByCountryId(long? countryId);

    /// <summary>
    /// Метод актуализации координаты страны
    /// </summary>
    /// <param cref="CountriesCoordinatesUpgradeRequest?" name="request">Модель запроса актуализации координаты страны</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="BaseResponse">Стандартный ответ</returns>
    /// <remarks>Новый идентификатор координаты страны</remarks>
    /// <exception cref="Exception">Исключение</exception>
    Task<BaseResponse> Upgrade(CountriesCoordinatesUpgradeRequest? request, string username);
}