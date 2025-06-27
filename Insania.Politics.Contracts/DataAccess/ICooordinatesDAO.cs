using Insania.Politics.Entities;

namespace Insania.Politics.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным координат
/// </summary>
public interface ICoordinatesDAO
{
    /// <summary>
    /// Метод получения списка координат
    /// </summary>
    /// <returns cref="List{CoordinatePolitics}">Список координат</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<CoordinatePolitics>> GetList();
}