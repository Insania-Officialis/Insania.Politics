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
    /// Метод получения координаты страны по идентификатору
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты страны</param>
    /// <returns cref="CountryCoordinate?">Координата страны</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<CountryCoordinate?> GetById(long? id)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetByIdCountryCoordinateMethod);

            //Проверки
            if (id == null) throw new Exception(ErrorMessagesPolitics.NotFoundCountryCoordinate);

            //Получение данных из бд
            CountryCoordinate? data = await _context.CountriesCoordinates
                .Where(x => x.Id == id)
                .Include(x => x.CountryEntity)
                .Include(x => x.CoordinateEntity)
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
    /// Метод получения координаты страны по идентификаторам страны и координаты
    /// </summary>
    /// <param cref="long?" name="countryId">Идентификатор страны</param>
    /// <param cref="long?" name="coordinateId">Идентификатор координаты</param>
    /// <returns cref="CountryCoordinate?">Координата страны</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<CountryCoordinate?> GetByCountryIdAndCoordinateId(long? countryId, long? coordinateId)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetByCountryIdAndCoordinateIdCountryCoordinateMethod);

            //Проверки
            if (countryId == null) throw new Exception(ErrorMessagesPolitics.NotFoundCountry);
            if (coordinateId == null) throw new Exception(ErrorMessagesPolitics.NotFoundCoordinate);

            //Получение данных из бд
            CountryCoordinate? data = await _context.CountriesCoordinates
                .Where(x => x.CountryId == countryId
                    && x.CoordinateId == coordinateId
                )
                .OrderBy(x => x.DateDeleted)
                .Include(x => x.CountryEntity)
                .Include(x => x.CoordinateEntity)
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
            List<CountryCoordinate> data = await _context
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

    /// <summary>
    /// Метод добавления координаты страны
    /// </summary>
    /// <param cref="Country?" name="country">Страна</param>
    /// <param cref="CoordinatePolitics?" name="coordinate">Координаты</param>
    /// <param cref="int?" name="zoom">Коэффициент масштаба отображения сущности</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="long?">Идентификатор координаты страны</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<long?> Add(Country? country, CoordinatePolitics? coordinate, int? zoom, string username)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredAddCountryCoordinateMethod);

            //Проверки
            if (country == null) throw new Exception(ErrorMessagesPolitics.NotFoundCountry);
            if (coordinate == null) throw new Exception(ErrorMessagesPolitics.NotFoundCoordinate);
            if (zoom == null) throw new Exception(ErrorMessagesPolitics.EmptyZoom);
            if (country.DateDeleted != null) throw new Exception(ErrorMessagesPolitics.DeletedCountry);
            if (coordinate.DateDeleted != null) throw new Exception(ErrorMessagesPolitics.DeletedCoordinate);
            if (zoom < 3 || zoom > 24) throw new Exception(ErrorMessagesPolitics.IncorrectZoom);

            //Получение данных из бд
            CountryCoordinate? data = await GetByCountryIdAndCoordinateId(country.Id, coordinate.Id);

            //Проверки
            if (data != null) throw new Exception(ErrorMessagesPolitics.ExistsCountryCoordinate);

            //Запись данных в бд
            CountryCoordinate CountryCoordinate = new(username, false, coordinate.PolygonEntity.InteriorPoint, coordinate.PolygonEntity.Area, zoom ?? 3, coordinate, country);
            await _context.CountriesCoordinates.AddAsync(CountryCoordinate);
            await _context.SaveChangesAsync();

            //Возврат результата
            return CountryCoordinate.Id;
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
    /// Метод восстановления координаты страны
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты страны</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="bool">Признак успешности</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<bool> Restore(long? id, string username)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredRestoreCountryCoordinateMethod);

            //Проверки
            if (id == null) throw new Exception(ErrorMessagesPolitics.NotFoundCountryCoordinate);

            //Получение данных из бд
            CountryCoordinate data = await GetById(id) ?? throw new Exception(ErrorMessagesPolitics.NotFoundCountryCoordinate);

            //Проверки
            if (data.DateDeleted == null) throw new Exception(ErrorMessagesPolitics.NotDeletedCountryCoordinate);

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
    /// Метод закрытия координаты страны
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты страны</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="bool">Признак успешности</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<bool> Close(long? id, string username)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredCloseCountryCoordinateMethod);

            //Проверки
            if (id == null) throw new Exception(ErrorMessagesPolitics.NotFoundCountryCoordinate);

            //Получение данных из бд
            CountryCoordinate data = await GetById(id) ?? throw new Exception(ErrorMessagesPolitics.NotFoundCountryCoordinate);

            //Проверки
            if (data.DateDeleted != null) throw new Exception(ErrorMessagesPolitics.DeletedCountryCoordinate);

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