using System.Transactions;

using Microsoft.Extensions.Logging;

using AutoMapper;
using NetTopologySuite.Geometries;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Models.Responses.Base;

using Insania.Politics.Contracts.BusinessLogic;
using Insania.Politics.Contracts.DataAccess;
using Insania.Politics.Entities;
using Insania.Politics.Models.Requests.CountriesCoordinates;
using Insania.Politics.Models.Responses.CountriesCoordinates;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesPolitics = Insania.Politics.Messages.ErrorMessages;
using InformationMessages = Insania.Politics.Messages.InformationMessages;

namespace Insania.Politics.BusinessLogic;

/// <summary>
/// Сервис работы с бизнес-логикой координат стран
/// </summary>
/// <param cref="ILogger{PoliticsObjectsCoordinatesBL}" name="logger">Сервис логгирования</param>
/// <param cref="IMapper" name="mapper">Сервис преобразования моделей</param>
/// <param cref="ICountriesCoordinatesDAO" name="countriesCoordinatesDAO">Сервис работы с данными координат стран</param>
/// <param cref="ICoordinatesDAO" name="coordinatesDAO">Сервис работы с данными координат</param>
/// <param cref="ICountriesDAO" name="countriesDAO">Сервис работы с данными стран</param>
/// <param cref="IPolygonParserSL" name="polygonParserSL">Сервис преобразования полигона</param>
public class CountriesCoordinatesBL(ILogger<CountriesCoordinatesBL> logger, IMapper mapper, ICountriesCoordinatesDAO countriesCoordinatesDAO, ICoordinatesDAO coordinatesDAO, ICountriesDAO countriesDAO, IPolygonParserSL polygonParserSL) : ICountriesCoordinatesBL
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<CountriesCoordinatesBL> _logger = logger;

    /// <summary>
    /// Сервис преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Сервис работы с данными координат стран
    /// </summary>
    private readonly ICountriesCoordinatesDAO _countriesCoordinatesDAO = countriesCoordinatesDAO;

    /// <summary>
    /// Сервис работы с данными стран
    /// </summary>
    private readonly ICountriesDAO _countriesDAO = countriesDAO;

    /// <summary>
    /// Сервис работы с данными координат
    /// </summary>
    private readonly ICoordinatesDAO _coordinatesDAO = coordinatesDAO;

    /// <summary>
    /// Сервис преобразования полигона
    /// </summary>
    private readonly IPolygonParserSL _polygonParserSL = polygonParserSL;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка координат стран по идентификатору страны
    /// </summary>
    /// <param cref="long?" name="сщгтекнId">Идентификатор страны</param>
    /// <returns cref="CountriesCoordinatesResponseList">Список координат стран</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<CountriesCoordinatesResponseList> GetList(long? countryId)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListCountriesCoordinatesMethod);

            //Проверки
            if (countryId == null) throw new Exception(ErrorMessagesPolitics.NotFoundCountry);

            //Получение данных
            List<CountryCoordinate>? data = await _countriesCoordinatesDAO.GetList(countryId);
            CountryCoordinate countryCoordinate = data?
                .OrderByDescending(x => x.Area)
                .FirstOrDefault() ?? throw new Exception(ErrorMessagesPolitics.NotFoundCountryCoordinate);
            Country country = countryCoordinate.CountryEntity ?? throw new Exception(ErrorMessagesPolitics.NotFoundCountry);

            //Формирование ответа
            CountriesCoordinatesResponseList? response = null;
            if (data == null) response = new(false);
            else
            {
                response = new
                (
                    true,
                    country.Id,
                    country.Name,
                    countryCoordinate.Center,
                    countryCoordinate.Zoom,
                    data?.Select(x => new CountriesCoordinatesResponseListItem(
                        x.Id,
                        x.CoordinateId,
                        _polygonParserSL.FromPolygonToDoubleArray(x.CoordinateEntity?.PolygonEntity),
                        x.CountryEntity?.Color,
                        x.CoordinateEntity?.TypeEntity?.BorderColor
                    )).ToList()
                );
            }

            //Возврат ответа
            return response;
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
    /// Метод актуализации координаты страны
    /// </summary>
    /// <param cref="CountriesCoordinatesUpgradeRequest?" name="request">Модель запроса актуализации координаты страны</param>
    /// <param cref="long?" name="coordinateId">Идентификатор координаты</param>
    /// <param cref="double[][][]?" name="coordinates">Координаты</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="BaseResponse">Стандартный ответ</returns>
    /// <remarks>Новый идентификатор координаты страны</remarks>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<BaseResponse> Upgrade(CountriesCoordinatesUpgradeRequest? request, string username)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredUpgradeCountryCoordinateMethod);

            //Проверки
            if (request == null) throw new Exception(ErrorMessagesShared.EmptyRequest);
            if (request.CountryId == null) throw new Exception(ErrorMessagesPolitics.NotFoundCountry);
            if (request.CoordinateId == null) throw new Exception(ErrorMessagesPolitics.NotFoundCoordinate);
            if (request.Coordinates == null) throw new Exception(ErrorMessagesShared.EmptyCoordinates);
            Polygon polygon = _polygonParserSL.FromDoubleArrayToPolygon(request.Coordinates) ?? throw new Exception(ErrorMessagesShared.IncorrectCoordinates);

            //Получение данных
            Country country = await _countriesDAO.GetById(request.CountryId) ?? throw new Exception(ErrorMessagesPolitics.NotFoundCountry);
            CoordinatePolitics coordinate = await _coordinatesDAO.GetById(request.CoordinateId) ?? throw new Exception(ErrorMessagesPolitics.NotFoundCoordinate);

            //Проверки
            if (country.DateDeleted != null) throw new Exception(ErrorMessagesPolitics.DeletedCountry);
            if (coordinate.DateDeleted != null) throw new Exception(ErrorMessagesPolitics.DeletedCoordinate);
            if (coordinate.PolygonEntity == polygon) throw new Exception(ErrorMessagesPolitics.NotChangesCoordinate);

            //Получение данных
            CountryCoordinate countryCoordinateOld = await _countriesCoordinatesDAO.GetByCountryIdAndCoordinateId(request.CountryId, request.CoordinateId) ?? throw new Exception(ErrorMessagesPolitics.NotFoundCountryCoordinate);

            //Проверки
            if (countryCoordinateOld.DateDeleted != null) throw new Exception(ErrorMessagesPolitics.DeletedCountryCoordinate);

            //Открытие транзакции
            using TransactionScope transactionScope = new(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TransactionManager.DefaultTimeout },
                TransactionScopeAsyncFlowOption.Enabled
            );

            //Закрытие старой координаты страны
            await _countriesCoordinatesDAO.Close(countryCoordinateOld.Id, username);

            //Создание новой координаты
            long? coordinateIdNew = await _coordinatesDAO.Add(polygon, countryCoordinateOld.CoordinateEntity?.TypeEntity as CoordinateTypePolitics, username);

            //Получение сущностей
            CoordinatePolitics coordinateNew = await _coordinatesDAO.GetById(coordinateIdNew) ?? throw new Exception(ErrorMessagesPolitics.NotFoundCoordinate);

            //Создание новой координаты страны
            long? result = await _countriesCoordinatesDAO.Add(country, coordinateNew, countryCoordinateOld.Zoom, username);

            //Фиксация транзакции
            transactionScope.Complete();

            //Возврат ответа
            return new(true, result);
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