using Insania.Politics.Entities;

namespace Insania.Politics.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным стран
/// </summary>
public interface ICountriesDAO
{
    /// <summary>
    /// Метод получения страны по идентификатору
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор страны</param>
    /// <returns cref="Country?">Страна</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<Country?> GetById(long? id);

    /// <summary>
    /// Метод получения списка стран
    /// </summary>
    /// <param cref="bool?" name="hasCoordinates">Проверка наличия координат</param>
    /// <returns cref="List{Country}">Список стран</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<Country>> GetList(bool? hasCoordinates = null);
}