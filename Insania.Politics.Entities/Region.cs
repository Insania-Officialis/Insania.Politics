using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Entities;

namespace Insania.Politics.Entities;

/// <summary>
/// Модель сущности региона
/// </summary>
[Table("c_regions")]
[Comment("Регионы")]
public class Region : Compendium
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности региона
    /// </summary>
    public Region() : base()
    {
        Description = string.Empty;
        Color = string.Empty;
    }

    /// <summary>
    /// Конструктор модели сущности региона без идентификатора
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="string" name="description">Описание</param>
    /// <param cref="string" name="color">Цвет на карте</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Region(ITransliterationSL transliteration, string username, string name, string description, string color, DateTime? dateDeleted = null) : base(transliteration, username, name, dateDeleted)
    {
        Description = description;
        Color = color;
    }

    /// <summary>
    /// Конструктор модели сущности региона с идентификатором
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="long?" name="id">Идентификатор пользователя</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="string" name="description">Описание</param>
    /// <param cref="string" name="color">Цвет на карте</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Region(ITransliterationSL transliteration, long id, string username, string name, string description, string color, DateTime? dateDeleted = null) : base(transliteration, id, username, name, dateDeleted)
    {
        Description = description;
        Color = color;
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
    #endregion
}