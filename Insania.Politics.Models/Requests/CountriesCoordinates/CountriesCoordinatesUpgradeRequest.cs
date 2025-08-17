using System.Text.Json.Serialization;

namespace Insania.Politics.Models.Requests.CountriesCoordinates;

/// <summary>
/// Модель запроса актуализации координаты страны
/// </summary>
/// <param cref="long?" name="countryId">Идентификатор страны</param>
/// <param cref="long?" name="coordinateId">Идентификатор координаты</param>
/// <param cref="string?" name="coordinates">Координаты</param>
public class CountriesCoordinatesUpgradeRequest(long? countryId, long? coordinateId, double[][][]? coordinates)
{
    /// <summary>
    /// Идентификатор страны
    /// </summary>
    [JsonPropertyName("politics_object_id")]
    public long? CountryId { get; set; } = countryId;

    /// <summary>
    /// Идентификатор координаты
    /// </summary>
    [JsonPropertyName("coordinate_id")]
    public long? CoordinateId { get; set; } = coordinateId;

    /// <summary>
    /// Координаты
    /// </summary>
    [JsonPropertyName("coordinates")]
    public double[][][]? Coordinates { get; set; } = coordinates;
}