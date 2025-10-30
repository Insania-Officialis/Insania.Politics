using NetTopologySuite.Geometries;

using Insania.Shared.Models.Responses.Base;

namespace Insania.Politics.Models.Responses.CountryCoordinates;

/// <summary>
/// Модель ответа списком координат страны
/// </summary>
/// <param cref="bool" name="success">Признак успешности</param>
/// <param cref="long?" name="id">Идентификатор страны</param>
/// <param cref="string?" name="name">Наименование страны</param>
/// <param cref="Point?" name="center">Центр страны</param>
/// <param cref="int?" name="zoom">Коэффициент масштаба отображения страны</param>
/// <param cref="List{CountriesCoordinatesResponseListItem}?" name="items">Список координат</param>
public class CountryCoordinatesResponseList(bool success, long? id = null, string? name = null, Point? center = null, int? zoom = null, List<CountryCoordinatesResponseListItem>? items = null) : BaseResponse(success, id)
{
    #region Поля
    /// <summary>
    /// Наименование страны
    /// </summary>
    public string? Name { get; set; } = name;

    /// <summary>
    /// Центр страны
    /// </summary>
    public Point? Center { get; set; } = center;

    /// <summary>
    /// Коэффициент масштаба отображения страны
    /// </summary>
    public int? Zoom { get; set; } = zoom;

    /// <summary>
    /// Список координат
    /// </summary>
    public List<CountryCoordinatesResponseListItem>? Items { get; set; } = items;
    #endregion
}