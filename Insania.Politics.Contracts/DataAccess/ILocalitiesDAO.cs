using Insania.Politics.Entities;

namespace Insania.Politics.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным населённых пунктов
/// </summary>
public interface ILocalitiesDAO
{
    /// <summary>
    /// Метод получения списка населённых пунктов
    /// </summary>
    /// <returns cref="List{Locality}">Список населённых пунктов</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<Locality>> GetList();
}