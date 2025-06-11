using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Entities;

namespace Insania.Politics.Entities;

/// <summary>
/// Модель сущности лога сервиса политики
/// </summary>
[Table("r_logs_api_politics")]
[Comment("Логи сервиса политики")]
public class LogApiPolitics : Log
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности лога сервиса политики
    /// </summary>
    public LogApiPolitics() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности лога сервиса политики без идентификатора
    /// </summary>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="string" name="method">Наименование вызываемого метода</param>
    /// <param cref="string" name="type">Тип вызываемого метода</param>
    /// <param cref="string" name="dataIn">Данные на вход</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public LogApiPolitics(string username, bool isSystem, string method, string type, string? dataIn = null, DateTime? dateDeleted = null) : base(username, isSystem, method, type, dataIn, dateDeleted)
    {

    }

    /// <summary>
    /// Конструктор модели сущности лога сервиса политики с идентификатором
    /// </summary>
    /// <param cref="long" name="id">Первичный ключ таблицы</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="string" name="method">Наименование вызываемого метода</param>
    /// <param cref="string" name="type">Тип вызываемого метода</param>
    /// <param cref="string" name="dataIn">Данные на вход</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public LogApiPolitics(long id, string username, bool isSystem, string method, string type, string? dataIn = null, DateTime? dateDeleted = null) : base(id, username, isSystem, method, type, dataIn, dateDeleted)
    {

    }
    #endregion
}