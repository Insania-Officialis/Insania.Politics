using Microsoft.Extensions.DependencyInjection;

using Insania.Politics.Contracts.DataAccess;
using Insania.Politics.Entities;
using Insania.Politics.Tests.Base;

namespace Insania.Politics.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными уровней населённых пунктов
/// </summary>
[TestFixture]
public class LocalitiesLevelsDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными уровней населённых пунктов
    /// </summary>
    private ILocalitiesLevelsDAO LocalitiesLevelsDAO { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        LocalitiesLevelsDAO = ServiceProvider.GetRequiredService<ILocalitiesLevelsDAO>();
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
    /// Тест метода получения списка уровней населённых пунктов
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<LocalityLevel>? result = await LocalitiesLevelsDAO.GetList();

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