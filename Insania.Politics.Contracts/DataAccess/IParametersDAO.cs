using Insania.Politics.Entities;

namespace Insania.Politics.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данными параметров
/// </summary>
public interface IParametersDAO
{
    /// <summary>
    /// Метод получения списка параметров
    /// </summary>
    /// <returns cref="List{ParameterPolitics}">Список параметров</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<ParameterPolitics>> GetList();
}