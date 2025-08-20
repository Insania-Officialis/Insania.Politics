using Insania.Politics.Entities;

namespace Insania.Politics.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным владений
/// </summary>
public interface IDomainsDAO
{
    /// <summary>
    /// Метод получения списка владений
    /// </summary>
    /// <returns cref="List{Domain}">Список владений</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<Domain>> GetList();
}