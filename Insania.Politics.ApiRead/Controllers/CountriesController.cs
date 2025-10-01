using Insania.Politics.Contracts.BusinessLogic;
using Insania.Shared.Messages;
using Insania.Shared.Models.Responses.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Insania.Politics.ApiRead.Controllers;

/// <summary>
/// Контроллер работы с странами
/// </summary>
/// <param cref="ILogger" name="logger">Сервис логгирования</param>
/// <param cref="IMemoryCache" name="memoryCache">Сервис кэширования</param>
/// <param cref="ICountriesBL" name="countriesBL">Сервис работы с бизнес-логикой стран</param>
[Route("countries")]
public class CountriesController(ILogger<CountriesController> logger, IMemoryCache memoryCache, ICountriesBL countriesBL) : Controller
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<CountriesController> _logger = logger;

    /// <summary>
    /// Сервис кэширования
    /// </summary>
    private readonly IMemoryCache _memoryCache = memoryCache;

    /// <summary>
    /// Сервис работы с бизнес-логикой стран
    /// </summary>
    private readonly ICountriesBL _countriesBL = countriesBL;
    #endregion

    #region Поля
    /// <summary>
    /// Класс для синхронизации потоков
    /// </summary>
    private static readonly SemaphoreSlim _cacheSemaphore = new(1, 1);
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
            //Получение результата
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

    /// <summary>
    /// Метод получения списка стран с координатами
    /// </summary>
    /// <returns cref="OkResult">Список стран с координатами</returns>
    /// <returns cref="BadRequestResult">Ошибка</returns>
    [HttpGet]
    [Route("list_with_coordinates")]
    public async Task<IActionResult> GetListWithCoordinates()
    {
        try
        {
            //Формирование ключа кэша
            string cacheKey = "countries";

            //Возврат результата при его наличии в кэше
            if (_memoryCache.TryGetValue(cacheKey, out string? cachedResult) && cachedResult != null) return Content(cachedResult, "application/json");

            //Установка блокировки
            await _cacheSemaphore.WaitAsync();

            try
            {
                //Возврат результата при его наличии в кэше после установки блокировки
                if (_memoryCache.TryGetValue(cacheKey, out cachedResult) && cachedResult != null) return Content(cachedResult, "application/json");

                //Получение результата
                BaseResponse? result = await _countriesBL.GetListWithCoordinates();

                //Сериализация ответа
                JsonSerializerSettings settings = new()
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    },
                    Formatting = Formatting.None
                };
                string serializedResult = JsonConvert.SerializeObject(result, settings);

                //Запись в кэш
                _memoryCache.Set(cacheKey, serializedResult, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10), Size = 1 });

                //Возврат результата
                return Content(serializedResult, "application/json");
            }
            finally
            {
                //Освобождение потока
                _cacheSemaphore.Release();
            };
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