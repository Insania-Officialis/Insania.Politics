using System.Text.Json;

using Microsoft.Extensions.DependencyInjection;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Models.Responses.Base;

using Insania.Politics.Contracts.BusinessLogic;
using Insania.Politics.Contracts.DataAccess;
using Insania.Politics.Entities;
using Insania.Politics.Models.Requests.CountriesCoordinates;
using Insania.Politics.Models.Responses.CountriesCoordinates;
using Insania.Politics.Tests.Base;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesPolitics = Insania.Politics.Messages.ErrorMessages;

namespace Insania.Politics.Tests.BusinessLogic;

/// <summary>
/// Тесты сервиса работы с бизнес-логикой координат стран
/// </summary>
[TestFixture]
public class CountriesCoordinatesBLTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Логин пользователя, выполняющего действие
    /// </summary>
    private readonly string _username = "test";
    #endregion

    #region Зависимости
    /// <summary>
    /// Сервис работы с бизнес-логикой координат стран
    /// </summary>
    private ICountriesCoordinatesBL CountriesCoordinatesBL { get; set; }

    /// <summary>
    /// Сервис работы с данными координат географических объектов
    /// </summary>
    private ICountriesCoordinatesDAO CountriesCoordinatesDAO { get; set; }

    /// <summary>
    /// Сервис преобразования полигона
    /// </summary>
    private IPolygonParserSL PolygonParserSL { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        CountriesCoordinatesBL = ServiceProvider.GetRequiredService<ICountriesCoordinatesBL>();
        CountriesCoordinatesDAO = ServiceProvider.GetRequiredService<ICountriesCoordinatesDAO>();
        PolygonParserSL = ServiceProvider.GetRequiredService<IPolygonParserSL>();
    }

    /// <summary>
    /// Метод, вызываемый после тестов
    /// </summary>
    [TearDown]
    public void TearDown()
    {

    }
    #endregion

    #region Методы тестирования
    /// <summary>
    /// Тест метода получения списка координат стран по идентификатору страны
    /// </summary>
    /// <param cref="long?" name="countryId">Идентификатор страны</param>
    [TestCase(null)]
    [TestCase(-1)]
    [TestCase(10000)]
    [TestCase(1)]
    public async Task GetListTest(long? countryId)
    {
        try
        {
            //Получение результата
            CountriesCoordinatesResponseList? result = await CountriesCoordinatesBL.GetList(countryId);

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            switch (countryId)
            {
                case 1:
                    Assert.That(string.IsNullOrWhiteSpace(result.Name), Is.False);
                    Assert.That(result.Center, Is.Not.Null);
                    Assert.That(result.Zoom, Is.Positive);
                    Assert.That(result.Items, Is.Not.Empty);
                    break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (countryId)
            {
                case null: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesPolitics.NotFoundCountry)); break;
                case -1: case 10000: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesPolitics.NotFoundCountryCoordinate)); break;
                default: throw;
            }
        }
    }

    /// <summary>
    /// Тест метода актуализации координаты страны
    /// </summary>
    /// <param cref="long?" name="countryId">Идентификатор страны</param>
    /// <param cref="long?" name="coordinateId">Идентификатор координаты</param>
    /// <param cref="string?" name="coordinates">Координаты</param>
    [TestCase(null, null, null)]
    [TestCase(-1, null, null)]
    [TestCase(-1, -1, null)]
    [TestCase(-1, -1, "[]")]
    [TestCase(-1, -1, "[[[0, 0],[0, 5],[5, 0]]]")]
    [TestCase(-1, -1, "[[[0,0],[0,5],[5,0],[0,0]]]")]
    [TestCase(10000, -1, "[[[0,0],[0,5],[5,0],[0,0]]]")]
    [TestCase(10000, 1, "[[[0,0],[0,5],[5,0],[0,0]]]")]
    [TestCase(1, 1, "[[[0,0],[0,5],[5,0],[0,0]]]")]
    [TestCase(1, 2, "[[[0,0],[0,5],[5,5],[5,0],[0,0]]]")]
    [TestCase(10001, 3, "[[[0,0],[0,5],[5,5],[5,0],[0,0]]]")]
    [TestCase(10001, 2, "[[[0,0],[0,5],[5,0],[0,0]]]")]
    [TestCase(1, 3, "[[[0,0],[0,20],[20,20],[20,0],[0,0]],[[5,5],[5,15],[15,15],[15,5],[5,5]]]")]
    public async Task UpgradeTest(long? countryId, long? coordinateId, string? coordinates)
    {
        try
        {
            //Получение значения до
            CountryCoordinate? countryCoordinateBefore = null;
            if (countryId != null && coordinateId != null) countryCoordinateBefore = await CountriesCoordinatesDAO.GetByCountryIdAndCoordinateId(countryId, coordinateId);

            //Формирование запроса
            double[][][]? polygon = null;
            if (!string.IsNullOrWhiteSpace(coordinates))
            {
                polygon = JsonSerializer.Deserialize<double[][][]>(coordinates);
            }
            CountriesCoordinatesUpgradeRequest? request = new(countryId, coordinateId, polygon);

            //Получение результата
            BaseResponse result = await CountriesCoordinatesBL.Upgrade(request, _username);

            //Получение значения после
            CountryCoordinate? countryCoordinateAfter = await CountriesCoordinatesDAO.GetById(result?.Id);

            //Проверка результата
            switch (countryId, coordinateId, coordinates)
            {
                case (1, 3, "[[[0,0],[0,20],[20,20],[20,0],[0,0]],[[5,5],[5,15],[15,15],[15,5],[5,5]]]"):
                    Assert.That(result, Is.Not.Null);
                    Assert.That(result?.Success, Is.True);
                    Assert.That(result?.Id, Is.Not.Null);
                    Assert.That(result?.Id, Is.Positive);
                    Assert.That(countryCoordinateBefore, Is.Not.Null);
                    Assert.That(countryCoordinateBefore?.CountryEntity, Is.Not.Null);
                    Assert.That(countryCoordinateBefore?.CoordinateEntity, Is.Not.Null);
                    Assert.That(countryCoordinateAfter, Is.Not.Null);
                    Assert.That(countryCoordinateAfter?.CountryEntity, Is.Not.Null);
                    Assert.That(countryCoordinateAfter?.CoordinateEntity, Is.Not.Null);
                    Assert.That(countryCoordinateBefore?.CountryEntity?.Id, Is.EqualTo(countryCoordinateAfter?.CountryEntity?.Id));
                    Assert.That(countryCoordinateBefore?.CoordinateEntity?.Id, Is.Not.EqualTo(countryCoordinateAfter?.CoordinateEntity?.Id));
                    Assert.That(countryCoordinateBefore?.CoordinateEntity?.PolygonEntity, Is.Not.EqualTo(countryCoordinateAfter?.CoordinateEntity?.PolygonEntity));
                    double[][][]? polygonAfter = PolygonParserSL.FromPolygonToDoubleArray(countryCoordinateAfter?.CoordinateEntity?.PolygonEntity);
                    Assert.That(polygonAfter, Is.EqualTo(polygon));
                    Assert.That(countryCoordinateBefore?.DateDeleted, Is.Not.Null);
                    Assert.That(countryCoordinateAfter?.DateDeleted, Is.Null);
                    double[][][]? polygonBefore = PolygonParserSL.FromPolygonToDoubleArray(countryCoordinateBefore?.CoordinateEntity?.PolygonEntity);
                    await CountriesCoordinatesDAO.Restore(countryCoordinateBefore?.Id, _username);
                    await CountriesCoordinatesDAO.Close(countryCoordinateAfter?.Id, _username);
                    break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (countryId, coordinateId, coordinates)
            {
                case (null, null, null): case (-1, -1, "[[[0,0],[0,5],[5,0],[0,0]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesPolitics.NotFoundCountry)); break;
                case (-1, null, null): case (10000, -1, "[[[0,0],[0,5],[5,0],[0,0]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesPolitics.NotFoundCoordinate)); break;
                case (-1, -1, null): case (-1, -1, "[]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesShared.EmptyCoordinates)); break;
                case (-1, -1, "[[[0, 0],[0, 5],[5, 0]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesShared.IncorrectCoordinates)); break;
                case (10000, 1, "[[[0,0],[0,5],[5,0],[0,0]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesPolitics.DeletedCountry)); break;
                case (1, 1, "[[[0,0],[0,5],[5,0],[0,0]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesPolitics.DeletedCoordinate)); break;
                case (1, 2, "[[[0,0],[0,5],[5,5],[5,0],[0,0]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesPolitics.NotFoundCountryCoordinate)); break;
                case (10001, 3, "[[[0,0],[0,5],[5,5],[5,0],[0,0]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesPolitics.DeletedCountryCoordinate)); break;
                case (2, 1, "[[[0,0],[0,5],[5,0],[0,0]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesPolitics.ExistsCountryCoordinate)); break;
                case (10001, 2, "[[[0,0],[0,5],[5,0],[0,0]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesPolitics.NotChangesCoordinate)); break;
                default: throw;
            }
        }
    }
    #endregion
}