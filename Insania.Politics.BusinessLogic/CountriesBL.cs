using Microsoft.Extensions.Logging;

using AutoMapper;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Models.Responses.Base;

using Insania.Politics.Contracts.BusinessLogic;
using Insania.Politics.Contracts.DataAccess;
using Insania.Politics.Entities;
using Insania.Politics.Models.Responses.Countries;
using Insania.Politics.Models.Responses.CountryCoordinates;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesPolitics = Insania.Politics.Messages.ErrorMessages;
using InformationMessages = Insania.Politics.Messages.InformationMessages;

namespace Insania.Politics.BusinessLogic;

/// <summary>
/// Сервис работы с бизнес-логикой стран
/// </summary>
/// <param cref="ILogger{CountriesBL}" name="logger">Сервис логгирования</param>
/// <param cref="IMapper" name="mapper">Сервис преобразования моделей</param>
/// <param cref="ICountriesDAO" name="countriesDAO">Сервис работы с данными стран</param>
/// <param cref="IPolygonParserSL" name="polygonParserSL">Сервис преобразования полигона</param>
public class CountriesBL(ILogger<CountriesBL> logger, IMapper mapper, ICountriesDAO countriesDAO, IPolygonParserSL polygonParserSL) : ICountriesBL
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<CountriesBL> _logger = logger;

    /// <summary>
    /// Сервис преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Сервис работы с данными стран
    /// </summary>
    private readonly ICountriesDAO _countriesDAO = countriesDAO;

    /// <summary>
    /// Сервис преобразования полигона
    /// </summary>
    private readonly IPolygonParserSL _polygonParserSL = polygonParserSL;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка стран
    /// </summary>
    /// <param cref="bool?" name="hasCoordinates">Проверка наличия координат</param>
    /// <returns cref="BaseResponseList">Стандартный ответ</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<BaseResponseList> GetList(bool? hasCoordinates = null)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListCountriesMethod);

            //Получение данных
            List<Country>? data = await _countriesDAO.GetList(hasCoordinates);

            //Формирование ответа
            BaseResponseList? response = null;
            if (data == null) response = new(false, null);
            else response = new(true, data?.Select(_mapper.Map<BaseResponseListItem>).ToList());

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
    /// Метод получения списка стран с координатами
    /// </summary>
    /// <returns cref="CountriesWithCoordinatesResponseList">Список стран с координатами</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<CountriesWithCoordinatesResponseList> GetListWithCoordinates()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListCountriesCoordinatesMethod);

            //Получение данных
            List<Country>? data = await _countriesDAO.GetList(true);

            //Формирование ответа
            CountriesWithCoordinatesResponseList? response = null;
            if (data == null) response = new(false);
            else response = new(
                true,
                [
                    .. data.Select(
                        x => new CountriesWithCoordinatesResponseListItem(
                            x.Id,
                            x.Name,
                            [
                                x.CountryCoordinates?.OrderByDescending(y => y.Area)?.FirstOrDefault()?.Center.X,
                                x.CountryCoordinates?.OrderByDescending(y => y.Area)?.FirstOrDefault()?.Center.Y
                            ],
                            x.CountryCoordinates?.OrderByDescending(y => y.Area)?.FirstOrDefault()?.Zoom,
                            [
                                ..x.CountryCoordinates?.Select(
                                    y => new CountryCoordinatesResponseListItem(
                                        y.Id,
                                        y.CoordinateId,
                                        _polygonParserSL.FromPolygonToDoubleArray(y.CoordinateEntity?.PolygonEntity),
                                        x.Color,
                                        y.CoordinateEntity?.TypeEntity?.BorderColor
                                    )
                                ) ?? []
                            ]
                        )
                    )
                ]
            );

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