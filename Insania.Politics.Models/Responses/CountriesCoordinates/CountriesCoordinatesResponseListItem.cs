﻿namespace Insania.Politics.Models.Responses.CountriesCoordinates;

/// <summary>
/// Модель элемента ответа списком координат стран
/// </summary>
/// <param cref="long?" name="id">Идентификатор координаты страны</param>
/// <param cref="long?" name="coordinateId">Идентификатор координаты</param>
/// <param cref="double[][][]?" name="coordinates">Координаты</param>
/// <param cref="string?" name="backgroundColor">Цвет фона</param>
/// <param cref="string?" name="borderColor">Цвет границ</param>
public class CountriesCoordinatesResponseListItem(long? id = null, long? coordinateId = null, double[][][]? coordinates = null, string? backgroundColor = null, string? borderColor = null)
{
    #region Поля
    /// <summary>
    /// Идентификатор координаты страны
    /// </summary>
    public long? Id { get; set; } = id;

    /// <summary>
    /// Идентификатор координаты
    /// </summary>
    public long? CoordinateId { get; set; } = coordinateId;

    /// <summary>
    /// Координаты
    /// </summary>
    public double[][][]? Coordinates { get; set; } = coordinates;

    /// <summary>
    /// Цвет фона
    /// </summary>
    public string? BackgroundColor { get; set; } = backgroundColor;

    /// <summary>
    /// Цвет границ
    /// </summary>
    public string? BorderColor { get; set; } = borderColor;
    #endregion
}