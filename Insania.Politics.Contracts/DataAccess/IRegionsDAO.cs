using Insania.Politics.Entities;

namespace Insania.Politics.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным регионов
/// </summary>
public interface IRegionsDAO
{
    /// <summary>
    /// Метод получения списка регионов
    /// </summary>
    /// <returns cref="List{Region}">Список регионов</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<Region>> GetList();
}