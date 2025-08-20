using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Politics.Contracts.DataAccess;
using Insania.Politics.Database.Contexts;
using Insania.Politics.Entities;
using Insania.Politics.Messages;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;

namespace Insania.Politics.DataAccess;

/// <summary>
/// Сервис работы с данными населённых пунктов
/// </summary>
/// <param cref="ILogger{LocalitiesDAO}" name="logger">Сервис логгирования</param>
/// <param cref="PoliticsContext" name="context">Контекст базы данных политики</param>
public class LocalitiesDAO(ILogger<LocalitiesDAO> logger, PoliticsContext context) : ILocalitiesDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<LocalitiesDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных политики
    /// </summary>
    private readonly PoliticsContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка населённых пунктов
    /// </summary>
    /// <returns cref="List{Locality}">Список населённых пунктов</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<Locality>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListLocalitiesMethod);

            //Получение данных из бд
            List<Locality> data = await _context.Localities.Where(x => x.DateDeleted == null).ToListAsync();

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