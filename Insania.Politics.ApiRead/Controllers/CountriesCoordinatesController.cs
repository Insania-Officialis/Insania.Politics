using Microsoft.AspNetCore.Mvc;

using Insania.Shared.Messages;
using Insania.Shared.Models.Responses.Base;

using Insania.Politics.Contracts.BusinessLogic;
using Insania.Politics.Models.Responses.CountryCoordinates;

namespace Insania.Politics.ApiRead.Controllers;

/// <summary>
/// Контроллер работы с координатами стран
/// </summary>
/// <param cref="ILogger" name="logger">Сервис логгирования</param>
/// <param cref="ICountriesCoordinatesBL" name="countriesCoordinatesBL">Сервис работы с бизнес-логикой координат стран</param>
[Route("countries_coordinates")]
public class CountriesCoordinatesController(ILogger<CountriesCoordinatesController> logger, ICountriesCoordinatesBL countriesCoordinatesBL) : Controller
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<CountriesCoordinatesController> _logger = logger;

    /// <summary>
    /// Сервис работы с бизнес-логикой координат стран
    /// </summary>
    private readonly ICountriesCoordinatesBL _countriesCoordinatesBL = countriesCoordinatesBL;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка координат страны по идентификатору страны
    /// </summary>
    /// <param cref="long" name="country_id">Идентификатор страны</param>
    /// <returns cref="OkResult">Список координат стран</returns>
    /// <returns cref="BadRequestResult">Ошибка</returns>
    [HttpGet]
    [Route("by_country_id")]
    public async Task<IActionResult> GetList([FromQuery] long? country_id)
    {
        try
        {
            //Получение результата
            CountryCoordinatesResponseList? result = await _countriesCoordinatesBL.GetByCountryId(country_id);

            //Возврат ответа
            return Ok(result);
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text} {ex}", ErrorMessages.Error, ex);

            //Возврат ошибки
            return BadRequest(new BaseResponseError(ex.Message));
        }
    }
    #endregion
}