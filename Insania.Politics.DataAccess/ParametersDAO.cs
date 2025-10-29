using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Politics.Contracts.DataAccess;
using Insania.Politics.Database.Contexts;
using Insania.Politics.Entities;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;

using InformationMessages = Insania.Politics.Messages.InformationMessages;

namespace Insania.Politics.DataAccess;

/// <summary>
/// Сервис работы с данными параметров
/// </summary>
/// <param cref="ILogger{ParametersDAO}" name="logger">Сервис логгирования</param>
/// <param cref="PoliticsContext" name="context">Контекст базы данных новостей</param>
public class ParametersDAO(ILogger<ParametersDAO> logger, PoliticsContext context) : IParametersDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<ParametersDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных новостей
    /// </summary>
    private readonly PoliticsContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка параметров
    /// </summary>
    /// <returns cref="List{ParameterPolitics}">Список параметров</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<ParameterPolitics>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListParametersMethod);

            //Получение данных из бд
            List<ParameterPolitics> data = await _context.Parameters.Where(x => x.DateDeleted == null).ToListAsync();

            //Возврат результата
            return data;
        }
        catch (Exception ex)
        {
            //Логгирование ошибки
            _logger.LogError("{text}: {error}", ErrorMessages.Error, ex.Message);

            //Проброс исключения
            throw;
        }
    }
    #endregion
}