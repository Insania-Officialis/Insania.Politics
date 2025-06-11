using Insania.Politics.Entities;

namespace Insania.Politics.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным типов организаций
/// </summary>
public interface IOrganizationsTypesDAO
{
    /// <summary>
    /// Метод получения списка типов организаций
    /// </summary>
    /// <returns cref="List{OrganizationType}">Список типов организаций</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<OrganizationType>> GetList();
}