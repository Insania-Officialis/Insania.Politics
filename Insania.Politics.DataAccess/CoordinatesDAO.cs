using Insania.Politics.Contracts.DataAccess;
using Insania.Politics.Database.Contexts;
using Insania.Politics.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using ErrorMessagesPolitics = Insania.Politics.Messages.ErrorMessages;
using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;
using InformationMessages = Insania.Politics.Messages.InformationMessages;

namespace Insania.Politics.DataAccess;

/// <summary>
/// Сервис работы с данными координат
/// </summary>
/// <param cref="ILogger{PoliticsDAO}" name="logger">Сервис логгирования</param>
/// <param cref="PoliticsContext" name="context">Контекст базы данных политики</param>
public class CoordinatesDAO(ILogger<CoordinatesDAO> logger, PoliticsContext context) : ICoordinatesDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<CoordinatesDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных политики
    /// </summary>
    private readonly PoliticsContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения координаты по идентификатору
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты</param>
    /// <returns cref="CoordinatePolitics?">Координата</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<CoordinatePolitics?> GetById(long? id)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetByIdCoordinateMethod);

            //Проверки
            if (id == null) throw new Exception(ErrorMessagesPolitics.NotFoundCoordinate);

            //Получение данных из бд
            CoordinatePolitics? data = await _context.Coordinates
                .Where(x => x.Id == id)
                .Include(x => x.TypeEntity)
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
    /// <returns cref="List{CoordinatePolitics}">Список стран</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<CoordinatePolitics>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListCoordinatesMethod);

            //Получение данных из бд
            List<CoordinatePolitics> data = await _context.Coordinates.Where(x => x.DateDeleted == null).ToListAsync();

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
    /// Метод добавление координаты
    /// </summary>
    /// <param cref="Polygon?" name="coordinates">Координаты</param>
    /// <param cref="CoordinateTypePolitics?" name="type">Тип координаты</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="long?">Идентификатор записи</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<long?> Add(Polygon? coordinates, CoordinateTypePolitics? type, string username)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredAddCoordinateMethod);

            //Проверки
            if (coordinates == null) throw new Exception(ErrorMessagesShared.EmptyCoordinates);
            if (type == null) throw new Exception(ErrorMessagesPolitics.NotFoundCoordinateType);
            if (type.DateDeleted != null) throw new Exception(ErrorMessagesPolitics.DeletedCoordinateType);

            //Запись данных в бд
            CoordinatePolitics entity = new(username, false, coordinates, type);
            _context.Add(entity);
            await _context.SaveChangesAsync();

            //Возврат результата
            return entity.Id;
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
    /// Метод восстановления координаты
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="bool">Признак успешности</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<bool> Restore(long? id, string username)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredRestoreCoordinateMethod);

            //Проверки
            if (id == null) throw new Exception(ErrorMessagesPolitics.NotFoundCoordinate);

            //Получение данных из бд
            CoordinatePolitics data = await GetById(id) ?? throw new Exception(ErrorMessagesPolitics.NotFoundCoordinate);

            //Проверки
            if (data.DateDeleted == null) throw new Exception(ErrorMessagesPolitics.NotDeletedCoordinate);

            //Запись данных в бд
            data.SetRestored();
            data.SetUpdate(username);
            _context.Update(data);
            await _context.SaveChangesAsync();

            //Возврат результата
            return true;
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
    /// Метод закрытия координаты
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="bool">Признак успешности</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<bool> Close(long? id, string username)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredCloseCoordinateMethod);

            //Проверки
            if (id == null) throw new Exception(ErrorMessagesPolitics.NotFoundCoordinate);

            //Получение данных из бд
            CoordinatePolitics data = await GetById(id) ?? throw new Exception(ErrorMessagesPolitics.NotFoundCoordinate);

            //Проверки
            if (data.DateDeleted != null) throw new Exception(ErrorMessagesPolitics.DeletedCoordinate);

            //Запись данных в бд
            data.SetDeleted();
            data.SetUpdate(username);
            _context.Update(data);
            await _context.SaveChangesAsync();

            //Возврат результата
            return true;
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