using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Entities;

namespace Insania.Politics.Entities;

/// <summary>
/// Модель сущности населённого пункта
/// </summary>
[Table("c_localities")]
[Comment("Населённые пункты")]
public class Locality : Compendium
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности населённого пункта
    /// </summary>
    public Locality() : base()
    {
        Description = string.Empty;
        AreaEntity = new();
        LocalityLevelEntity = new();
    }

    /// <summary>
    /// Конструктор модели сущности населённого пункта без идентификатора
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="string" name="description">Описание</param>
    /// <param cref="Area" name="area">Область</param>
    /// <param cref="LocalityLevel" name="localitylevel">Уровень населённого пункта</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Locality(ITransliterationSL transliteration, string username, string name, string description, Area area, LocalityLevel localitylevel, DateTime? dateDeleted = null) : base(transliteration, username, name, dateDeleted)
    {
        Description = description;
        AreaId = area.Id;
        AreaEntity = area;
        LocalityLevelId = localitylevel.Id;
        LocalityLevelEntity = localitylevel;
    }

    /// <summary>
    /// Конструктор модели сущности населённого пункта с идентификатором
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="long?" name="id">Идентификатор пользователя</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="string" name="description">Описание</param>
    /// <param cref="Area" name="area">Область</param>
    /// <param cref="LocalityLevel" name="localitylevel">Уровень населённого пункта</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Locality(ITransliterationSL transliteration, long id, string username, string name, string description, Area area, LocalityLevel localitylevel, DateTime? dateDeleted = null) : base(transliteration, id, username, name, dateDeleted)
    {
        Description = description;
        AreaId = area.Id;
        AreaEntity = area;
        LocalityLevelId = localitylevel.Id;
        LocalityLevelEntity = localitylevel;
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
    ///	Идентификатор области
    /// </summary>
    [Column("area_id")]
    [Comment("Идентификатор области")]
    public long AreaId { get; private set; }

    /// <summary>
    ///	Идентификатор уровня населённого пункта
    /// </summary>
    [Column("locality_level_id")]
    [Comment("Идентификатор населённого пункта")]
    public long LocalityLevelId { get; private set; }
    #endregion

    #region Навигационные свойства
    /// <summary>
    /// Навигационное свойство области
    /// </summary>
    [ForeignKey("AreaId")]
    public Area AreaEntity { get; private set; }

    /// <summary>
    /// Навигационное свойство уровня населённого пункта
    /// </summary>
    [ForeignKey("LocalityLevelId")]
    public LocalityLevel LocalityLevelEntity { get; private set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод записи описания
    /// </summary>
    /// <param cref="string" name="description">Описание</param>
    public void SetDescription(string description) => Description = description;

    /// <summary>
    /// Метод записи области
    /// </summary>
    /// <param cref="Area" name="area">Область</param>
    public void SetArea(Area area)
    {
        AreaId = area.Id;
        AreaEntity = area;
    }

    /// <summary>
    /// Метод записи уровня населённого пункта
    /// </summary>
    /// <param cref="LocalityLevel" name="localitylevel">Уровень населённого пункта</param>
    public void SetLocalityLevel(LocalityLevel localitylevel)
    {
        LocalityLevelId = localitylevel.Id;
        LocalityLevelEntity = localitylevel;
    }
    #endregion
}