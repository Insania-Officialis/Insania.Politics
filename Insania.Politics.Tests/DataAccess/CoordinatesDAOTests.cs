using Microsoft.Extensions.DependencyInjection;

using Insania.Politics.Contracts.DataAccess;
using Insania.Politics.Entities;
using Insania.Politics.Tests.Base;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesPolitics = Insania.Politics.Messages.ErrorMessages;

namespace Insania.Politics.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными координат
/// </summary>
[TestFixture]
public class CoordinatesDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Логин пользователя, выполняющего действие
    /// </summary>
    private readonly string _username = "test";
    #endregion

    #region Зависимости
    /// <summary>
    /// Сервис работы с данными координат
    /// </summary>
    private ICoordinatesDAO CoordinatesDAO { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        CoordinatesDAO = ServiceProvider.GetRequiredService<ICoordinatesDAO>();
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
            CoordinatePolitics? result = await CoordinatesDAO.GetById(id);

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
                case null: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesPolitics.NotFoundCoordinate)); break;
                default: throw;
            }
        }
    }

    /// <summary>
    /// Тест метода получения списка координат
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<CoordinatePolitics>? result = await CoordinatesDAO.GetList();

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
    /// Тест метода восстановления координаты
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты</param>
    [TestCase(null)]
    [TestCase(-1)]
    [TestCase(1)]
    [TestCase(2)]
    public async Task RestoreTest(long? id)
    {
        try
        {
            //Получение значения до
            CoordinatePolitics? coordinateBefore = null;
            if (id != null)
            {
                Shared.Entities.Coordinate? coordinate = await CoordinatesDAO.GetById(id);
                if (coordinate != null) coordinateBefore = new(coordinate);
            }

            //Получение результата
            bool? result = await CoordinatesDAO.Restore(id, _username);

            //Получение значения после
            CoordinatePolitics? coordinateAfter = null;
            if (id != null) coordinateAfter = await CoordinatesDAO.GetById(id);

            //Проверка результата
            switch (id)
            {
                case 1:
                    Assert.That(result, Is.True);
                    Assert.That(coordinateBefore, Is.Not.Null);
                    Assert.That(coordinateAfter, Is.Not.Null);
                    Assert.That(coordinateBefore!.Id, Is.EqualTo(coordinateAfter!.Id));
                    Assert.That(coordinateBefore!.DateCreate, Is.LessThan(coordinateAfter!.DateUpdate));
                    Assert.That(coordinateAfter!.DateDeleted, Is.Null);
                    await CoordinatesDAO.Close(id, _username);
                    break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (id)
            {
                case null: case -1: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesPolitics.NotFoundCoordinate)); break;
                case 2: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesPolitics.NotDeletedCoordinate)); break;
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
    [TestCase(1)]
    [TestCase(2)]
    public async Task CloseTest(long? id)
    {
        try
        {
            //Получение значения до
            CoordinatePolitics? coordinateBefore = null;
            if (id != null)
            {
                Shared.Entities.Coordinate? coordinate = await CoordinatesDAO.GetById(id);
                if (coordinate != null) coordinateBefore = new(coordinate);
            }

            //Получение результата
            bool? result = await CoordinatesDAO.Close(id, _username);

            //Получение значения после
            CoordinatePolitics? coordinateAfter = null;
            if (id != null) coordinateAfter = await CoordinatesDAO.GetById(id);

            //Проверка результата
            switch (id)
            {
                case 2:
                    Assert.That(result, Is.True);
                    Assert.That(coordinateBefore, Is.Not.Null);
                    Assert.That(coordinateAfter, Is.Not.Null);
                    Assert.That(coordinateBefore!.Id, Is.EqualTo(coordinateAfter!.Id));
                    Assert.That(coordinateBefore!.DateCreate, Is.LessThan(coordinateAfter!.DateUpdate));
                    Assert.That(coordinateAfter!.DateDeleted, Is.Not.Null);
                    await CoordinatesDAO.Restore(id, _username);
                    break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (id)
            {
                case null: case -1: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesPolitics.NotFoundCoordinate)); break;
                case 1: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesPolitics.DeletedCoordinate)); break;
                default: throw;
            }
        }
    }
    #endregion
}