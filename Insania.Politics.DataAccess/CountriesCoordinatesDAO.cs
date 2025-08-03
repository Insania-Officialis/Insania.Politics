using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Politics.Contracts.DataAccess;
using Insania.Politics.Database.Contexts;
using Insania.Politics.Entities;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using InformationMessages = Insania.Politics.Messages.InformationMessages;
using ErrorMessagesPolitics = Insania.Politics.Messages.ErrorMessages;

namespace Insania.Politics.DataAccess;

/// <summary>
/// Сервис работы с данными координат стран
/// </summary>
/// <param cref="ILogger{CountrysCoordinatesDAO}" name="logger">Сервис логгирования</param>
/// <param cref="PoliticsContext" name="context">Контекст базы данных политики</param>
public class CountriesCoordinatesDAO(ILogger<CountriesCoordinatesDAO> logger, PoliticsContext context) : ICountriesCoordinatesDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<CountriesCoordinatesDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных политики
    /// </summary>
    private readonly PoliticsContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка координат стран
    /// </summary>
    /// <returns cref="List{CountryCoordinate}">Список координат стран</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<CountryCoordinate>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListCountriesCoordinatesMethod);

            //Получение данных из бд
            List<CountryCoordinate> data = await _context.CountriesCoordinates
                .Include(x => x.CountryEntity)
                .Include(x => x.CoordinateEntity)
                .ThenInclude(y => y != null ? y.TypeEntity : null)
                .ToListAsync();

            //Возврат результата
            return data;
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text}: {error}", ErrorMessagesShared.Error, ex.Message);

            //Проброс исключения
            throw;
        }
    }

    /// <summary>
    /// Метод получения списка координат стран по идентификатору страны
    /// </summary>
    /// <param cref="long?" name="countryId">Идентификатор страны</param>
    /// <returns cref="List{CountryCoordinate}">Список координат стран</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<CountryCoordinate>> GetList(long? countryId)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListCountriesCoordinatesMethod);

            //Проверки
            if (countryId == null) throw new Exception(ErrorMessagesPolitics.NotFoundCountry);

            //Получение данных из бд
            List<CountryCoordinate> data = await context
                .CountriesCoordinates
                .Include(x => x.CountryEntity)
                .Include(x => x.CoordinateEntity)
                .ThenInclude(y => y != null ? y.TypeEntity : null)
                .Where(x => x.DateDeleted == null && x.CountryId == countryId)
                .ToListAsync();

            //Возврат результата
            return data;
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text}: {error}", ErrorMessagesShared.Error, ex.Message);

            //Проброс исключения
            throw;
        }
    }
    #endregion
}