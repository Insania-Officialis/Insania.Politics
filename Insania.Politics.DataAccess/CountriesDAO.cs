using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Politics.Contracts.DataAccess;
using Insania.Politics.Database.Contexts;
using Insania.Politics.Entities;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesPolitics = Insania.Politics.Messages.ErrorMessages;
using InformationMessages = Insania.Politics.Messages.InformationMessages;

namespace Insania.Politics.DataAccess;

/// <summary>
/// Сервис работы с данными стран
/// </summary>
/// <param cref="ILogger{PoliticsDAO}" name="logger">Сервис логгирования</param>
/// <param cref="PoliticsContext" name="context">Контекст базы данных политики</param>
public class CountriesDAO(ILogger<CountriesDAO> logger, PoliticsContext context) : ICountriesDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<CountriesDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных политики
    /// </summary>
    private readonly PoliticsContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения страны по идентификатору
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор страны</param>
    /// <returns cref="Country?">Страна</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<Country?> GetById(long? id)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetByIdCountryMethod);

            //Проверки
            if (id == null) throw new Exception(ErrorMessagesPolitics.NotFoundCountry);

            //Получение данных из бд
            Country? data = await _context.Countries
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

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
    /// Метод получения списка стран
    /// </summary>
    /// <param cref="bool?" name="hasCoordinates">Проверка наличия координат</param>
    /// <returns cref="List{Country}">Список стран</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<Country>> GetList(bool? hasCoordinates = null)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListCountriesMethod);

            //Формирование запроса
            IQueryable<Country> query = _context.Countries.Where(x => x.DateDeleted == null);
            if (hasCoordinates.HasValue)
            {
                if (hasCoordinates.Value) query = query.Where(x => x.CountryCoordinates!.Any(y => y.DateDeleted == null));
                else query = query.Where(x => !x.CountryCoordinates!.Any(y => y.DateDeleted == null));
            }
            if (hasCoordinates == true) query = query.Include(x => x.CountryCoordinates!.Where(y => y.DateDeleted == null)).ThenInclude(y => y.CoordinateEntity).ThenInclude(z => z!.TypeEntity);

            //Получение данных из бд
            List<Country> data = await query.ToListAsync();

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