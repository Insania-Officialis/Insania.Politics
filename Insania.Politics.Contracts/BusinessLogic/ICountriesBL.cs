using Insania.Shared.Models.Responses.Base;

namespace Insania.Politics.Contracts.BusinessLogic;

/// <summary>
/// Интерфейс работы с бизнес-логикой стран
/// </summary>
public interface ICountriesBL
{
    /// <summary>
    /// Метод получения списка стран
    /// </summary>
    /// <param cref="bool?" name="hasCoordinates">Проверка наличия координат</param>
    /// <returns cref="BaseResponseList">Стандартный ответ</returns>
    /// <remarks>Список стран</remarks>
    /// <exception cref="Exception">Исключение</exception>
    Task<BaseResponseList> GetList(bool? hasCoordinates = null);
}