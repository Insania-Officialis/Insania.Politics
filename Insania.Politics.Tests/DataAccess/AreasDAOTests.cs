using Microsoft.Extensions.DependencyInjection;

using Insania.Politics.Contracts.DataAccess;
using Insania.Politics.Entities;
using Insania.Politics.Tests.Base;

namespace Insania.Politics.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными областей
/// </summary>
[TestFixture]
public class AreasDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными областей
    /// </summary>
    private IAreasDAO AreasDAO { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        AreasDAO = ServiceProvider.GetRequiredService<IAreasDAO>();
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
    /// Тест метода получения списка областей
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<Area>? result = await AreasDAO.GetList();

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