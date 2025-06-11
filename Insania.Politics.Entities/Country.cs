using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Entities;

namespace Insania.Politics.Entities;

/// <summary>
/// Модель сущности страны
/// </summary>
[Table("d_countries")]
[Comment("Страны")]
public class Country : Compendium
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности страны
    /// </summary>
    public Country() : base()
    {
        Description = string.Empty;
        LanguageForNames = string.Empty;
        Color = string.Empty;
        OrganizationEntity = new();
    }

    /// <summary>
    /// Конструктор модели сущности страны без идентификатора
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="string" name="description">Описание</param>
    /// <param cref="string" name="languageForNames">Язык для названий</param>
    /// <param cref="string" name="color">Цвет на карте</param>
    /// <param cref="Organization" name="organization">Организация</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Country(ITransliterationSL transliteration, string username, string name, string description, string languageForNames, string color, Organization organization, DateTime? dateDeleted = null) : base(transliteration, username, name, dateDeleted)
    {
        Description = description;
        LanguageForNames = languageForNames;
        Color = color;
        OrganizationId = organization.Id;
        OrganizationEntity = organization;
    }

    /// <summary>
    /// Конструктор модели сущности страны с идентификатором
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="long?" name="id">Идентификатор пользователя</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="string" name="description">Описание</param>
    /// <param cref="string" name="languageForNames">Язык для названий</param>
    /// <param cref="string" name="color">Цвет на карте</param>
    /// <param cref="Organization" name="organization">Организация</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Country(ITransliterationSL transliteration, long id, string username, string name, string description, string languageForNames, string color, Organization organization, DateTime? dateDeleted = null) : base(transliteration, id, username, name, dateDeleted)
    {
        Description = description;
        LanguageForNames = languageForNames;
        Color = color;
        OrganizationId = organization.Id;
        OrganizationEntity = organization;
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
    ///	Язык для названий
    /// </summary>
    [Column("language_for_names")]
    [Comment("Язык для названий")]
    public string LanguageForNames { get; private set; }

    /// <summary>
    ///	Цвет на карте
    /// </summary>
    [Column("color")]
    [Comment("Цвет на карте")]
    public string Color { get; private set; }

    /// <summary>
    ///	Идентификатор организации
    /// </summary>
    [Column("organization_id")]
    [Comment("Идентификатор организации")]
    [ForeignKey(nameof(OrganizationEntity))]
    public long OrganizationId { get; private set; }
    #endregion

    #region Навигационные свойства
    /// <summary>
    /// Навигационное свойство организации
    /// </summary>
    public Organization OrganizationEntity { get; private set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод записи описания
    /// </summary>
    /// <param cref="string" name="description">Описание</param>
    public void SetDescription(string description) => Description = description;

    /// <summary>
    /// Метод записи языка для названий
    /// </summary>
    /// <param cref="string" name="languageForNames">Язык для названий</param>
    public void SetLanguageForNames(string languageForNames) => LanguageForNames = languageForNames;

    /// <summary>
    /// Метод записи цвета на карте
    /// </summary>
    /// <param cref="string" name="color">Цвет на карте</param>
    public void SetColor(string color) => Color = color;

    /// <summary>
    /// Метод записи организации
    /// </summary>
    /// <param cref="Organization" name="organization">Организация</param>
    public void SetOrganization(Organization organization)
    {
        OrganizationId = organization.Id;
        OrganizationEntity = organization;
    }
    #endregion
}