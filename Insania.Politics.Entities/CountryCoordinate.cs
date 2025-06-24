using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using NetTopologySuite.Geometries;

using Insania.Shared.Entities;

namespace Insania.Politics.Entities;

/// <summary>
/// Модель сущности координаты страны
/// </summary>
[Table("u_countries_coordinates")]
[Comment("Координаты стран")]
public class CountryCoordinate : EntityCoordinate
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности координаты страны
    /// </summary>
    public CountryCoordinate() : base()
    {
        CountryEntity = new();
    }

    /// <summary>
    /// Конструктор модели сущности координаты страны без идентификатора
    /// </summary>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="Point" name="center">Координаты точки центра сущности</param>
    /// <param cref="int" name="zoom">Коэффициент масштаба отображения сущности</param>
    /// <param cref="CoordinatePolitics" name="coordinate">Координата</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public CountryCoordinate(string username, bool isSystem, Point center, int zoom, CoordinatePolitics coordinate, Country country, DateTime? dateDeleted = null) : base(username, isSystem, center, zoom, coordinate, dateDeleted)
    {
        CountryId = country.Id;
        CountryEntity = country;
    }

    /// <summary>
    /// Конструктор модели сущности координаты страны с идентификатором
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор пользователя</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="Point" name="center">Координаты точки центра сущности</param>
    /// <param cref="int" name="zoom">Коэффициент масштаба отображения сущности</param>
    /// <param cref="CoordinatePolitics" name="coordinate">Координата</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public CountryCoordinate(long id, string username, bool isSystem, Point center, int zoom, CoordinatePolitics coordinate, Country country, DateTime? dateDeleted = null) : base(id, username, isSystem, center, zoom, coordinate, dateDeleted)
    {
        CountryId = country.Id;
        CountryEntity = country;
    }
    #endregion

    #region Поля
    /// <summary>
    /// Идентификатор страны
    /// </summary>
    [Column("country_id")]
    [Comment("Идентификатор страны")]
    public long CountryId { get; private set; }
    #endregion

    #region Навигационные свойства
    /// <summary>
    /// Навигационное свойство страны
    /// </summary>
    [ForeignKey("CountryId")]
    public Country? CountryEntity { get; private set; }
    #endregion

    #region Методы

    /// <summary>
    /// Метод записи координаты
    /// </summary>
    /// <param cref="Country" name="country">Страна</param>
    public void SetCountry(Country country)
    {
        CountryId = country.Id;
        CountryEntity = country;
    }
    #endregion
}