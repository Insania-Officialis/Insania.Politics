using Microsoft.Extensions.DependencyInjection;

using Insania.Shared.Contracts.DataAccess;

using Insania.Politics.Contracts.DataAccess;
using Insania.Politics.Entities;
using Insania.Politics.Tests.Base;

namespace Insania.Politics.Tests.DataAccess;

/// <summary>
/// Тесты сервиса инициализации данных в бд файлов
/// </summary>
[TestFixture]
public class InitializationDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис инициализации данных в бд файлов
    /// </summary>
    private IInitializationDAO InitializationDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными типов организаций
    /// </summary>
    private IOrganizationsTypesDAO OrganizationsTypesDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными организаций
    /// </summary>
    private IOrganizationsDAO OrganizationsDAO { get; set; }

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
        InitializationDAO = ServiceProvider.GetRequiredService<IInitializationDAO>();
        OrganizationsTypesDAO = ServiceProvider.GetRequiredService<IOrganizationsTypesDAO>();
        OrganizationsDAO = ServiceProvider.GetRequiredService<IOrganizationsDAO>();
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
    /// Тест метода инициализации данных
    /// </summary>
    [Test]
    public async Task InitializeTest()
    {
        try
        {
            //Выполнение метода
            await InitializationDAO.Initialize();

            //Получение сущностей
            List<OrganizationType> organizationsTypes = await OrganizationsTypesDAO.GetList();
            List<Organization> organizations = await OrganizationsDAO.GetList();
            List<Country> countries = await CountriesDAO.GetList();

            //Проверка результата
            Assert.Multiple(() =>
            {
                Assert.That(organizationsTypes, Is.Not.Empty);
                Assert.That(organizations, Is.Not.Empty);
                Assert.That(countries, Is.Not.Empty);
            });
        }
        catch (Exception)
        {
            //Проброс исключения
            throw;
        }
    }
    #endregion
}