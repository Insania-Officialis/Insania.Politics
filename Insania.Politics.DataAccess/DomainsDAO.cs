using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Politics.Contracts.DataAccess;
using Insania.Politics.Database.Contexts;
using Insania.Politics.Entities;
using Insania.Politics.Messages;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;

namespace Insania.Politics.DataAccess;

/// <summary>
/// Сервис работы с данными владений
/// </summary>
/// <param cref="ILogger{DomainsDAO}" name="logger">Сервис логгирования</param>
/// <param cref="PoliticsContext" name="context">Контекст базы данных политики</param>
public class DomainsDAO(ILogger<DomainsDAO> logger, PoliticsContext context) : IDomainsDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<DomainsDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных политики
    /// </summary>
    private readonly PoliticsContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка владений
    /// </summary>
    /// <returns cref="List{Domain}">Список владений</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<Domain>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListDomainsMethod);

            //Получение данных из бд
            List<Domain> data = await _context.Domains.Where(x => x.DateDeleted == null).ToListAsync();

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