using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Politics.Contracts.DataAccess;
using Insania.Politics.Database.Contexts;
using Insania.Politics.Entities;
using Insania.Politics.Messages;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;

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
                query = query
                    .Include(x => x.CountryCoordinates)
                    .Where(x => (hasCoordinates ?? false)
                        ? x.CountryCoordinates != null &&
                          x.CountryCoordinates.Any(y => y.DateDeleted == null)
                        : x.CountryCoordinates == null ||
                          !x.CountryCoordinates.Any(y => y.DateDeleted == null)
                    );
            }

            //Получение данных из бд
            List<Country> data = await query.ToListAsync();

            //Возврат результата
            return data;
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text}: {error}", ErrorMessages.Error, ex.Message);

            //Проброс исключения
            throw;
        }

    }
    #endregion
}