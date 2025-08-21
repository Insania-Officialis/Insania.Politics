using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Entities;

namespace Insania.Politics.Entities;

/// <summary>
/// Модель сущности уровня населённого пункта
/// </summary>
[Table("c_localities_levels")]
[Comment("Уровни населённых пунктов")]
public class LocalityLevel : Compendium
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности уровня населённого пункта
    /// </summary>
    public LocalityLevel() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности уровня населённого пункта без идентификатора
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="int" name="minSize">Минимальный размер</param>
    /// <param cref="int" name="maxSize">Максимальный размер</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public LocalityLevel(ITransliterationSL transliteration, string username, string name, int minSize, int maxSize, DateTime? dateDeleted = null) : base(transliteration, username, name, dateDeleted)
    {
        MinSize = minSize;
        MaxSize = maxSize;
    }

    /// <summary>
    /// Конструктор модели сущности уровня населённого пункта с идентификатором
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="long?" name="id">Идентификатор пользователя</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="int" name="minSize">Минимальный размер</param>
    /// <param cref="int" name="maxSize">Максимальный размер</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public LocalityLevel(ITransliterationSL transliteration, long id, string username, string name, int minSize, int maxSize, DateTime? dateDeleted = null) : base(transliteration, id, username, name, dateDeleted)
    {
        MinSize = minSize;
        MaxSize = maxSize;
    }
    #endregion

    #region Поля
    /// <summary>
    ///	Минимальный размер
    /// </summary>
    [Column("min_size")]
    [Comment("Минимальный размер")]
    public int MinSize { get; private set; }

    /// <summary>
    ///	Максимальный размер
    /// </summary>
    [Column("max_size")]
    [Comment("Максимальный размер")]
    public int MaxSize { get; private set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод записи минимального размера
    /// </summary>
    /// <param cref="int" name="minSize">Минимальный размер</param>
    public void SetMinSize(int minSize) => MinSize = minSize;

    /// <summary>
    /// Метод записи максимального размера
    /// </summary>
    /// <param cref="int" name="maxSize">Максимальный размер</param>
    public void SetMaxSize(int maxSize) => MaxSize = maxSize;
    #endregion
}