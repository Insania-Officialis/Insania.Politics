using Insania.Politics.Entities;

namespace Insania.Politics.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данными типов координат
/// </summary>
public interface ICoordinatesTypesDAO
{
    /// <summary>
    /// Метод получения типа координаты по идентификатору
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор типа координаты</param>
    /// <returns cref="CoordinateTypePolitics?">Тип координаты</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<CoordinateTypePolitics?> GetById(long? id);

    /// <summary>
    /// Метод получения списка типов координат
    /// </summary>
    /// <returns cref="List{CoordinateTypePolitics}">Список типов координат</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<CoordinateTypePolitics>> GetList();
}