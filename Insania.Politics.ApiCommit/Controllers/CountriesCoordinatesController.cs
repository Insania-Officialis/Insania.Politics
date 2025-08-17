using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Insania.Shared.Messages;
using Insania.Shared.Models.Responses.Base;

using Insania.Politics.Contracts.BusinessLogic;
using Insania.Politics.Models.Requests.CountriesCoordinates;

namespace Insania.Politics.ApiCommit.Controllers;

/// <summary>
/// Контроллер работы с координатами стран
/// </summary>
/// <param cref="ILogger" name="logger">Сервис логгирования</param>
/// <param cref="ICountriesCoordinatesBL" name="countriesCoordinatesBL">Сервис работы с бизнес-логикой координат стран</param>
[Authorize]
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
    /// Метод получения списка координат стран
    /// </summary>
    /// <param cref="CountriesCoordinatesUpgradeRequest" name="request">Модель запроса актуализации координаты географического объекта</param>
    /// <returns cref="OkResult">Список координат стран</returns>
    /// <returns cref="BadRequestResult">Ошибка</returns>
    [HttpPost]
    [Route("upgrade")]
    public async Task<IActionResult> Upgrade([FromBody] CountriesCoordinatesUpgradeRequest? request)
    {
        try
        {
            //Проверки
            if (request == null) throw new Exception(ErrorMessages.EmptyRequest);

            //Получение текущего пользователя
            string username = User?.Identity?.Name ?? throw new Exception(ErrorMessages.NotFoundCurrentUser);

            //Получение результата
            BaseResponse? result = await _countriesCoordinatesBL.Upgrade(request, username);

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