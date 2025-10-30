using Insania.Shared.Models.Responses.Base;

namespace Insania.Politics.Models.Responses.Countries;

/// <summary>
/// Модель ответа списком стран с координатами
/// </summary>
/// <param cref="bool" name="success">Признак успешности</param>
/// <param cref="List{CountriesWithCoordinatesResponseListItem}?" name="items">Список стран с координатами</param>
public class CountriesWithCoordinatesResponseList(bool success, List<CountriesWithCoordinatesResponseListItem>? items = null) : BaseResponse(success)
{
    /// <summary>
    /// Список координат стран
    /// </summary>
    public List<CountriesWithCoordinatesResponseListItem>? Items { get; set; } = items;
}