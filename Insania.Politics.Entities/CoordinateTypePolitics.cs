using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Entities;

namespace Insania.Politics.Entities;

/// <summary>
/// Модель сущности типа координаты политики
/// </summary>
[Table("d_coordinates_types")]
[Comment("Логи сервиса политики")]
public class CoordinateTypePolitics : CoordinateType
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности типа координаты политики
    /// </summary>
    public CoordinateTypePolitics() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности типа координаты политики без идентификатора
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public CoordinateTypePolitics(ITransliterationSL transliteration, string username, string name, DateTime? dateDeleted = null) : base(transliteration, username, name, dateDeleted)
    {

    }

    /// <summary>
    /// Конструктор модели сущности типа координаты политики с идентификатором
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="long" name="id">Первичный ключ таблицы</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public CoordinateTypePolitics(ITransliterationSL transliteration, long id, string username, string name, DateTime? dateDeleted = null) : base(transliteration, id, username, name, dateDeleted)
    {

    }
    #endregion
}