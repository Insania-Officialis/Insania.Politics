using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Entities;

namespace Insania.Politics.Entities;

/// <summary>
/// Модель сущности области
/// </summary>
[Table("c_areas")]
[Comment("Области")]
public class Area : Compendium
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности области
    /// </summary>
    public Area() : base()
    {
        Description = string.Empty;
        Color = string.Empty;
        CountryEntity = new();
        RegionEntity = new();
        DomainEntity = new();
    }

    /// <summary>
    /// Конструктор модели сущности области без идентификатора
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="string" name="description">Описание</param>
    /// <param cref="string" name="color">Цвет на карте</param>
    /// <param cref="Country" name="country">Страна</param>
    /// <param cref="Region" name="region">Регион</param>
    /// <param cref="Domain" name="domain">Владение</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Area(ITransliterationSL transliteration, string username, string name, string description, string color, Country country, Region region, Domain domain, DateTime? dateDeleted = null) : base(transliteration, username, name, dateDeleted)
    {
        Description = description;
        Color = color;
        CountryId = country.Id;
        CountryEntity = country;
        RegionId = region.Id;
        RegionEntity = region;
        DomainId = domain.Id;
        DomainEntity = domain;
    }

    /// <summary>
    /// Конструктор модели сущности области с идентификатором
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="long?" name="id">Идентификатор пользователя</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="string" name="description">Описание</param>
    /// <param cref="string" name="color">Цвет на карте</param>
    /// <param cref="Country" name="country">Страна</param>
    /// <param cref="Region" name="region">Регион</param>
    /// <param cref="Domain" name="domain">Владение</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Area(ITransliterationSL transliteration, long id, string username, string name, string description, string color, Country country, Region region, Domain domain, DateTime? dateDeleted = null) : base(transliteration, id, username, name, dateDeleted)
    {
        Description = description;
        Color = color;
        CountryId = country.Id;
        CountryEntity = country;
        RegionId = region.Id;
        RegionEntity = region;
        DomainId = domain.Id;
        DomainEntity = domain;
    }
    #endregion

    #region Поля
    /// <summary>
    ///	Описание
    /// </summary>
    [Column("description")]
    [Comment("Описание")]
    public string Description { get; private set; }

    /// <summary>
    ///	Цвет на карте
    /// </summary>
    [Column("color")]
    [Comment("Цвет на карте")]
    public string Color { get; private set; }

    /// <summary>
    /// Идентификатор страны
    /// </summary>
    [Column("country_id")]
    [Comment("Идентификатор страны")]
    public long CountryId { get; private set; }

    /// <summary>
    /// Идентификатор региона
    /// </summary>
    [Column("region_id")]
    [Comment("Идентификатор региона")]
    public long RegionId { get; private set; }

    /// <summary>
    /// Идентификатор владения
    /// </summary>
    [Column("domain_id")]
    [Comment("Идентификатор владения")]
    public long DomainId { get; private set; }
    #endregion

    #region Навигационные свойства
    /// <summary>
    /// Навигационное свойство страны
    /// </summary>
    [ForeignKey("CountryId")]
    public Country CountryEntity { get; private set; }

    /// <summary>
    /// Навигационное свойство региона
    /// </summary>
    [ForeignKey("RegionId")]
    public Region RegionEntity { get; private set; }

    /// <summary>
    /// Навигационное свойство владения
    /// </summary>
    [ForeignKey("DomainId")]
    public Domain DomainEntity { get; private set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод записи описания
    /// </summary>
    /// <param cref="string" name="description">Описание</param>
    public void SetDescription(string description) => Description = description;

    /// <summary>
    /// Метод записи цвета на карте
    /// </summary>
    /// <param cref="string" name="color">Цвет на карте</param>
    public void SetColor(string color) => Color = color;

    /// <summary>
    /// Метод записи страны
    /// </summary>
    /// <param cref="Country" name="country">Страна</param>
    public void SetCountry(Country country)
    {
        CountryId = country.Id;
        CountryEntity = country;
    }

    /// <summary>
    /// Метод записи региона
    /// </summary>
    /// <param cref="Region" name="region">Регион</param>
    public void SetRegion(Region region)
    {
        RegionId = region.Id;
        RegionEntity = region;
    }

    /// <summary>
    /// Метод записи владения
    /// </summary>
    /// <param cref="Domain" name="domain">Владение</param>
    public void SetDomain(Domain domain)
    {
        DomainId = domain.Id;
        DomainEntity = domain;
    }
    #endregion
}