using Microsoft.Extensions.Logging;

using AutoMapper;

using Insania.Shared.Contracts.Services;

using Insania.Politics.Contracts.BusinessLogic;
using Insania.Politics.Contracts.DataAccess;
using Insania.Politics.Entities;
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
/// <param cref="IPolygonParserSL" name="polygonParserSL">Сервис преобразования полигона</param>
public class CountriesCoordinatesBL(ILogger<CountriesCoordinatesBL> logger, IMapper mapper, ICountriesCoordinatesDAO countriesCoordinatesDAO, IPolygonParserSL polygonParserSL) : ICountriesCoordinatesBL
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
    #endregion
}