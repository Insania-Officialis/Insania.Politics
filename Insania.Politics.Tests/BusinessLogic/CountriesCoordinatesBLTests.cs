using Microsoft.Extensions.DependencyInjection;

using Insania.Shared.Contracts.Services;

using Insania.Politics.Contracts.BusinessLogic;
using Insania.Politics.Models.Responses.CountriesCoordinates;
using Insania.Politics.Tests.Base;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesPolitics = Insania.Politics.Messages.ErrorMessages;

namespace Insania.Politics.Tests.BusinessLogic;

/// <summary>
/// Тесты сервиса работы с бизнес-логикой координат географических объектов
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
    /// Сервис работы с бизнес-логикой координат географических объектов
    /// </summary>
    private ICountriesCoordinatesBL CountriesCoordinatesBL { get; set; }

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
    /// Тест метода получения списка координат географических объектов по идентификатору географического объекта
    /// </summary>
    /// <param cref="long?" name="countryId">Идентификатор географического объекта</param>
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
    #endregion
}