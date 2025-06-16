using Microsoft.EntityFrameworkCore;

using Insania.Politics.Entities;

namespace Insania.Politics.Database.Contexts;

/// <summary>
/// Контекст бд логов сервиса политики
/// </summary>
public class LogsApiPoliticsContext : DbContext
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор контекста бд логов сервиса политики
    /// </summary>
    public LogsApiPoliticsContext() : base()
    {

    }

    /// <summary>
    /// Конструктор контекста бд логов сервиса политики с опциями
    /// </summary>
    /// <param cref="DbContextOptions{LogsApiPoliticsContext}" name="options">Параметры</param>
    public LogsApiPoliticsContext(DbContextOptions<LogsApiPoliticsContext> options) : base(options)
    {

    }
    #endregion

    #region Поля
    /// <summary>
    /// Логи
    /// </summary>
    public virtual DbSet<LogApiPolitics> Logs { get; set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод при создании моделей
    /// </summary>
    /// <param cref="ModelBuilder" name="modelBuilder">Конструктор моделей</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Установка схемы бд
        modelBuilder.HasDefaultSchema("insania_logs_api_politics");
        
        //Добавление gin-индекса на поле с входными данными логов
        modelBuilder.Entity<LogApiPolitics>().HasIndex(x => x.DataIn).HasMethod("gin");

        //Добавление gin-индекса на поле с выходными данными логов
        modelBuilder.Entity<LogApiPolitics>().HasIndex(x => x.DataOut).HasMethod("gin");
    }
    #endregion
}