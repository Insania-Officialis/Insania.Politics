using Insania.Politics.Entities;

namespace Insania.Politics.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным областей
/// </summary>
public interface IAreasDAO
{
    /// <summary>
    /// Метод получения списка областей
    /// </summary>
    /// <returns cref="List{Area}">Список областей</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<Area>> GetList();
}