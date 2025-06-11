using Insania.Politics.Entities;

namespace Insania.Politics.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным организаций
/// </summary>
public interface IOrganizationsDAO
{
    /// <summary>
    /// Метод получения списка организаций
    /// </summary>
    /// <returns cref="List{Organization}">Список организаций</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<Organization>> GetList();
}