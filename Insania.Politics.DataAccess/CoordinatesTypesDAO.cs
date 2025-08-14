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
/// Сервис работы с данными типов координат
/// </summary>
/// <param cref="ILogger{CoordinatesTypesDAO}" name="logger">Сервис логгирования</param>
/// <param cref="PoliticsContext" name="context">Контекст базы данных географии</param>
public class CoordinatesTypesDAO(ILogger<CoordinatesTypesDAO> logger, PoliticsContext context) : ICoordinatesTypesDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<CoordinatesTypesDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных географии
    /// </summary>
    private readonly PoliticsContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения типа координаты по идентификатору
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор типа координаты</param>
    /// <returns cref="CoordinateTypePolitics?">Тип координаты</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<CoordinateTypePolitics?> GetById(long? id)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetByIdCoordinateTypeMethod);

            //Проверки
            if (id == null) throw new Exception(ErrorMessagesPolitics.NotFoundCoordinateType);

            //Получение данных из бд
            CoordinateTypePolitics? data = await _context.CoordinatesTypes
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
    /// Метод получения списка типов координат
    /// </summary>
    /// <returns cref="List{CoordinateTypePolitics}">Список типов координат</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<CoordinateTypePolitics>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListCoordinatesTypesMethod);

            //Получение данных из бд
            List<CoordinateTypePolitics> data = await _context.CoordinatesTypes
                .Where(x => x.DateDeleted == null)
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