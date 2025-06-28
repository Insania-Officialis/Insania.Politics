using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using NetTopologySuite.Geometries;

using Coordinate = Insania.Shared.Entities.Coordinate;

namespace Insania.Politics.Entities;

/// <summary>
/// Модель сущности координаты политики
/// </summary>
[Table("r_coordinates")]
[Comment("Координаты политики")]
public class CoordinatePolitics : Coordinate
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности координаты политики
    /// </summary>
    public CoordinatePolitics() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности координаты политики без идентификатора
    /// </summary>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="Polygon" name="polygon">Полигон (массив координат)</param>
    /// <param cref="CoordinateTypePolitics" name="type">Тип</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public CoordinatePolitics(string username, bool isSystem, Polygon polygon, CoordinateTypePolitics type, DateTime? dateDeleted = null) : base(username, isSystem, polygon, type, dateDeleted)
    {

    }

    /// <summary>
    /// Конструктор модели сущности координаты политики с идентификатором
    /// </summary>
    /// <param cref="long" name="id">Первичный ключ таблицы</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="Polygon" name="polygon">Полигон (массив координат)</param>
    /// <param cref="CoordinateTypePolitics" name="type">Тип</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public CoordinatePolitics(long id, string username, bool isSystem, Polygon polygon, CoordinateTypePolitics type, DateTime? dateDeleted = null) : base(id, username, isSystem, polygon, type, dateDeleted)
    {

    }
    #endregion
}