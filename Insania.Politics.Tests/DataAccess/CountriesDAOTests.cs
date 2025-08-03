using Microsoft.Extensions.DependencyInjection;

using Insania.Politics.Contracts.DataAccess;
using Insania.Politics.Entities;
using Insania.Politics.Tests.Base;

namespace Insania.Politics.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными стран
/// </summary>
[TestFixture]
public class CountriesDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными стран
    /// </summary>
    private ICountriesDAO CountriesDAO { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        CountriesDAO = ServiceProvider.GetRequiredService<ICountriesDAO>();
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
    /// Тест метода получения списка стран
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<Country>? result = await CountriesDAO.GetList();

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
    /// Тест метода получения списка стран с проверкой наличия координат
    /// </summary>
    /// <param cref="bool?" name="hasCoordinates">Проверка наличия координат</param>
    [TestCase(false)]
    [TestCase(true)]
    public async Task GetListWithCheckCoordinatesTest(bool hasCoordinates)
    {
        try
        {
            //Получение результата
            List<Country>? result = await CountriesDAO.GetList(hasCoordinates: hasCoordinates);

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
    #endregion
}