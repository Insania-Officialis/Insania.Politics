using Microsoft.AspNetCore.Mvc;

using Insania.Shared.Messages;
using Insania.Shared.Models.Responses.Base;

using Insania.Politics.Contracts.BusinessLogic;

namespace Insania.Politics.ApiRead.Controllers;

/// <summary>
/// Контроллер работы с странами
/// </summary>
/// <param cref="ILogger" name="logger">Сервис логгирования</param>
/// <param cref="ICountriesBL" name="countriesBL">Сервис работы с бизнес-логикой стран</param>
[Route("countries")]
public class CountriesController(ILogger<CountriesController> logger, ICountriesBL countriesBL) : Controller
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<CountriesController> _logger = logger;

    /// <summary>
    /// Сервис работы с бизнес-логикой стран
    /// </summary>
    private readonly ICountriesBL _countriesBL = countriesBL;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка стран
    /// </summary>
    /// <param cref="bool" name="has_coordinates">Проверка наличия координат</param>
    /// <returns cref="OkResult">Список стран</returns>
    /// <returns cref="BadRequestResult">Ошибка</returns>
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetList([FromQuery] bool? has_coordinates = null)
    {
        try
        {
            //Получение результата проверки логина
            BaseResponse? result = await _countriesBL.GetList(has_coordinates);

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