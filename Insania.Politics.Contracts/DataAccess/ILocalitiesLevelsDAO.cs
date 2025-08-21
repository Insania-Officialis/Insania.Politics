using Insania.Politics.Entities;

namespace Insania.Politics.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным уровней населённых пунктов
/// </summary>
public interface ILocalitiesLevelsDAO
{
    /// <summary>
    /// Метод получения списка уровней населённых пунктов
    /// </summary>
    /// <returns cref="List{LocalityLevel}">Список уровней населённых пунктов</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<LocalityLevel>> GetList();
}