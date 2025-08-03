using Microsoft.Extensions.DependencyInjection;

using Insania.Politics.Contracts.DataAccess;
using Insania.Politics.DataAccess;
using Insania.Politics.Entities;
using Insania.Politics.Tests.Base;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesPolitics = Insania.Politics.Messages.ErrorMessages;

namespace Insania.Politics.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными координат стран
/// </summary>
[TestFixture]
public class CountriesCoordinatesDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными координат стран
    /// </summary>
    private ICountriesCoordinatesDAO CountriesCoordinatesDAO { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        CountriesCoordinatesDAO = ServiceProvider.GetRequiredService<ICountriesCoordinatesDAO>();
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
    /// Тест метода получения списка координат стран
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<CountryCoordinate>? result = await CountriesCoordinatesDAO.GetList();

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }
        catch (Exception)
        {
            //Проброс исключения
            throw;
        }
    }

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
            List<CountryCoordinate>? result = await CountriesCoordinatesDAO.GetList(countryId);

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            switch (countryId)
            {
                case -1: case 10000: Assert.That(result, Is.Empty); break;
                case 1: Assert.That(result, Is.Not.Empty); Assert.That(!result.Any(x => x.CoordinateEntity == null)); break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (countryId)
            {
                case null: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesPolitics.NotFoundCountry)); break;
                default: throw;
            }
        }
    }
    #endregion
}