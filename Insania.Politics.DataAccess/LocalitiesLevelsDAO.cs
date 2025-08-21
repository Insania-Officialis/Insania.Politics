using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Politics.Contracts.DataAccess;
using Insania.Politics.Database.Contexts;
using Insania.Politics.Entities;
using Insania.Politics.Messages;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;

namespace Insania.Politics.DataAccess;

/// <summary>
/// Сервис работы с данными уровней населённых пунктов
/// </summary>
/// <param cref="ILogger{LocalitiesLevelsDAO}" name="logger">Сервис логгирования</param>
/// <param cref="PoliticsContext" name="context">Контекст базы данных политики</param>
public class LocalitiesLevelsDAO(ILogger<LocalitiesLevelsDAO> logger, PoliticsContext context) : ILocalitiesLevelsDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<LocalitiesLevelsDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных политики
    /// </summary>
    private readonly PoliticsContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка уровней населённых пунктов
    /// </summary>
    /// <returns cref="List{LocalityLevel}">Список уровней населённых пунктов</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<LocalityLevel>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListLocalitiesLevelsMethod);

            //Получение данных из бд
            List<LocalityLevel> data = await _context.LocalitiesLevels.Where(x => x.DateDeleted == null).ToListAsync();

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