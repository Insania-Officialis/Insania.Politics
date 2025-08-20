using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Politics.Contracts.DataAccess;
using Insania.Politics.Database.Contexts;
using Insania.Politics.Entities;
using Insania.Politics.Messages;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;

namespace Insania.Politics.DataAccess;

/// <summary>
/// Сервис работы с данными регионов
/// </summary>
/// <param cref="ILogger{RegionsDAO}" name="logger">Сервис логгирования</param>
/// <param cref="PoliticsContext" name="context">Контекст базы данных политики</param>
public class RegionsDAO(ILogger<RegionsDAO> logger, PoliticsContext context) : IRegionsDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<RegionsDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных политики
    /// </summary>
    private readonly PoliticsContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка регионов
    /// </summary>
    /// <returns cref="List{Region}">Список регионов</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<Region>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListRegionsMethod);

            //Получение данных из бд
            List<Region> data = await _context.Regions.Where(x => x.DateDeleted == null).ToListAsync();

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