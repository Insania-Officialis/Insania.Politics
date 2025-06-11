using Insania.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Insania.Politics.Entities;

/// <summary>
/// Модель сущности организации
/// </summary>
[Table("r_organizations")]
[Comment("Организации")]
public class Organization : Reestr
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности организации
    /// </summary>
    public Organization() : base()
    {
        Name = string.Empty;
        TypeEntity = new();
    }

    /// <summary>
    /// Конструктор модели сущности организации без идентификатора
    /// </summary>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="OrganizationType" name="type">Тип</param>
    /// <param cref="Organization?" name="parent">Родитель</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Organization(string username, string name, bool isSystem, OrganizationType type, Organization? parent = null, DateTime? dateDeleted = null) : base(username, isSystem, dateDeleted)
    {
        Name = name;
        TypeId = type.Id;
        TypeEntity = type;
        ParentId = parent?.Id;
        ParentEntity = parent;
    }

    /// <summary>
    /// Конструктор модели сущности организации с идентификатором
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор пользователя</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="OrganizationType" name="type">Тип</param>
    /// <param cref="Organization?" name="parent">Родитель</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Organization(long id, string username, string name, bool isSystem, OrganizationType type, Organization? parent = null, DateTime? dateDeleted = null) : base(id, username, isSystem, dateDeleted)
    {
        Name = name;
        TypeId = type.Id;
        TypeEntity = type;
        ParentId = parent?.Id;
        ParentEntity = parent;
    }
    #endregion

    #region Поля
    /// <summary>
    /// Наименование 
    /// </summary>
    [Column("name")]
    [Comment("Наименование")]
    public string Name { get; private set; }

    /// <summary>
    ///	Идентификатор типа
    /// </summary>
    [Column("type_id")]
    [Comment("Идентификатор типа")]
    [ForeignKey(nameof(TypeEntity))]
    public long TypeId { get; private set; }

    /// <summary>
    ///	Идентификатор родителя
    /// </summary>
    [Column("parent_id")]
    [Comment("Идентификатор родителя")]
    [ForeignKey(nameof(ParentEntity))]
    public long? ParentId { get; private set; }
    #endregion

    #region Навигационные свойства
    /// <summary>
    /// Навигационное свойство типа
    /// </summary>
    public OrganizationType TypeEntity { get; private set; }

    /// <summary>
    /// Навигационное свойство родителя
    /// </summary>
    public Organization? ParentEntity { get; private set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод записи наименования
    /// </summary>
    /// <param cref="string" name="name">Наименование</param>
    public void SetName(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Метод записи типа
    /// </summary>
    /// <param cref="OrganizationType" name="type">Тип</param>
    public void SetType(OrganizationType type)
    {
        TypeId = type.Id;
        TypeEntity = type;
    }

    /// <summary>
    /// Метод записи родителя
    /// </summary>
    /// <param cref="Organization?" name="parent">Родитель</param>
    public void SetParent(Organization? parent)
    {
        ParentId = parent?.Id;
        ParentEntity = parent;
    }
    #endregion
}