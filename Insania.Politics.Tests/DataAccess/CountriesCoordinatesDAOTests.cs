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
    /// Логин пользователя, выполняющего действие
    /// </summary>
    private readonly string _username = "test";
    #endregion

    #region Зависимости
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
    /// Тест метода получения координаты по идентификатору
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты</param>
    [TestCase(null)]
    [TestCase(-1)]
    [TestCase(1)]
    [TestCase(2)]
    public async Task GetByIdTest(long? id)
    {
        try
        {
            //Получение результата
            CountryCoordinate? result = await CountriesCoordinatesDAO.GetById(id);

            //Проверка результата
            switch (id)
            {
                case -1: Assert.That(result, Is.Null); break;
                case 1: case 2: Assert.That(result, Is.Not.Null); break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (id)
            {
                case null: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesPolitics.NotFoundCountryCoordinate)); break;
                default: throw;
            }
        }
    }

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

    /// <summary>
    /// Тест метода восстановления координаты
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты</param>
    [TestCase(null)]
    [TestCase(-1)]
    [TestCase(10000)]
    [TestCase(2)]
    public async Task RestoreTest(long? id)
    {
        try
        {
            //Получение значения до
            CountryCoordinate? CountryCoordinateBefore = null;
            if (id != null)
            {
                CountryCoordinateBefore = await CountriesCoordinatesDAO.GetById(id);
            }

            //Получение результата
            bool? result = await CountriesCoordinatesDAO.Restore(id, _username);

            //Получение значения после
            CountryCoordinate? CountryCoordinateAfter = null;
            if (id != null) CountryCoordinateAfter = await CountriesCoordinatesDAO.GetById(id);

            //Проверка результата
            switch (id)
            {
                case 10000:
                    Assert.That(result, Is.True);
                    Assert.That(CountryCoordinateBefore, Is.Not.Null);
                    Assert.That(CountryCoordinateAfter, Is.Not.Null);
                    Assert.That(CountryCoordinateBefore!.Id, Is.EqualTo(CountryCoordinateAfter!.Id));
                    Assert.That(CountryCoordinateBefore!.DateCreate, Is.LessThan(CountryCoordinateAfter!.DateUpdate));
                    Assert.That(CountryCoordinateAfter!.DateDeleted, Is.Null);
                    await CountriesCoordinatesDAO.Close(id, _username);
                    break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (id)
            {
                case null: case -1: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesPolitics.NotFoundCountryCoordinate)); break;
                case 2: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesPolitics.NotDeletedCountryCoordinate)); break;
                default: throw;
            }
        }
    }

    /// <summary>
    /// Тест метода закрытия координаты
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты</param>
    [TestCase(null)]
    [TestCase(-1)]
    [TestCase(10000)]
    [TestCase(2)]
    public async Task CloseTest(long? id)
    {
        try
        {
            //Получение значения до
            CountryCoordinate? CountryCoordinateBefore = null;
            if (id != null)
            {
                CountryCoordinateBefore = await CountriesCoordinatesDAO.GetById(id);
            }

            //Получение результата
            bool? result = await CountriesCoordinatesDAO.Close(id, _username);

            //Получение значения после
            CountryCoordinate? CountryCoordinateAfter = null;
            if (id != null) CountryCoordinateAfter = await CountriesCoordinatesDAO.GetById(id);

            //Проверка результата
            switch (id)
            {
                case 2:
                    Assert.That(result, Is.True);
                    Assert.That(CountryCoordinateBefore, Is.Not.Null);
                    Assert.That(CountryCoordinateAfter, Is.Not.Null);
                    Assert.That(CountryCoordinateBefore!.Id, Is.EqualTo(CountryCoordinateAfter!.Id));
                    Assert.That(CountryCoordinateBefore!.DateCreate, Is.LessThan(CountryCoordinateAfter!.DateUpdate));
                    Assert.That(CountryCoordinateAfter!.DateDeleted, Is.Not.Null);
                    await CountriesCoordinatesDAO.Restore(id, _username);
                    break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (id)
            {
                case null: case -1: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesPolitics.NotFoundCountryCoordinate)); break;
                case 10000: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesPolitics.DeletedCountryCoordinate)); break;
                default: throw;
            }
        }
    }
    #endregion
}