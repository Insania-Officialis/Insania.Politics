using Microsoft.Extensions.DependencyInjection;

using Insania.Politics.Contracts.DataAccess;
using Insania.Politics.Entities;
using Insania.Politics.Tests.Base;

namespace Insania.Politics.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными населённых пунктов
/// </summary>
[TestFixture]
public class LocalitiesDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными населённых пунктов
    /// </summary>
    private ILocalitiesDAO LocalitiesDAO { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        LocalitiesDAO = ServiceProvider.GetRequiredService<ILocalitiesDAO>();
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
    /// Тест метода получения списка населённых пунктов
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<Locality>? result = await LocalitiesDAO.GetList();

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