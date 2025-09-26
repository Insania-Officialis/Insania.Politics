using Insania.Politics.Models.Responses.CountryCoordinates;

namespace Insania.Politics.Models.Responses.Countries;

/// <summary>
/// Модель ответа списком стран с координатами
/// </summary>
/// <param cref="bool" name="success">Признак успешности</param>
/// <param cref="long?" name="id">Идентификатор страны</param>
/// <param cref="string?" name="name">Наименование страны</param>
/// <param cref="double?[]?" name="center">Центр страны</param>
/// <param cref="int?" name="zoom">Коэффициент масштаба отображения страны</param>
/// <param cref="List{GeographyObjectCoordinatesResponseListItem}?" name="coordinates">Список координат</param>

public class CountriesWithCoordinatesResponseListItem(long? id = null, string? name = null, double?[]? center = null, int? zoom = null, List<CountryCoordinatesResponseListItem>? coordinates = null)
{
    /// <summary>
    /// Идентификатор страны
    /// </summary>
    public long? Id { get; set; } = id;

    /// <summary>
    /// Наименование стран
    /// </summary>
    public string? Name { get; set; } = name;

    /// <summary>
    /// Центр страны
    /// </summary>
    public double?[]? Center { get; set; } = center;

    /// <summary>
    /// Коэффициент масштаба отображения страны
    /// </summary>
    public int? Zoom { get; set; } = zoom;

    /// <summary>
    /// Список координат
    /// </summary>
    public List<CountryCoordinatesResponseListItem>? Coordinates { get; set; } = coordinates;
}