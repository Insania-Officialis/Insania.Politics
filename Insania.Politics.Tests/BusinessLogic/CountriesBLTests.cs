using Microsoft.Extensions.DependencyInjection;

using Insania.Shared.Models.Responses.Base;

using Insania.Politics.Contracts.BusinessLogic;
using Insania.Politics.Tests.Base;

namespace Insania.Politics.Tests.BusinessLogic;

/// <summary>
/// Тесты сервиса работы с бизнес-логикой стран
/// </summary>
[TestFixture]
public class CountriesBLTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с бизнес-логикой стран
    /// </summary>
    private ICountriesBL CountriesBL { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        CountriesBL = ServiceProvider.GetRequiredService<ICountriesBL>();
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
            BaseResponseList? result = await CountriesBL.GetList();

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            Assert.That(result.Success, Is.True);
            Assert.That(result.Items, Is.Not.Null);
            Assert.That(result.Items, Is.Not.Empty);
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
            BaseResponseList? result = await CountriesBL.GetList(hasCoordinates);

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            Assert.That(result.Success, Is.True);
            Assert.That(result.Items, Is.Not.Null);
            Assert.That(result.Items, Is.Not.Empty);
        }
        catch (Exception)
        {
            //Проброс исключения
            throw;
        }
    }
    #endregion
}