using Insania.Politics.Entities;

namespace Insania.Politics.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным стран
/// </summary>
public interface ICountriesDAO
{
    /// <summary>
    /// Метод получения списка стран
    /// </summary>
    /// <returns cref="List{Country}">Список стран</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<Country>> GetList();
}