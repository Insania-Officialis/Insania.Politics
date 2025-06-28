using Microsoft.EntityFrameworkCore;

using Insania.Shared.Entities;

using Insania.Politics.Entities;

namespace Insania.Politics.Database.Contexts;

/// <summary>
/// Контекст бд политики
/// </summary>
public class PoliticsContext : DbContext
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор контекста бд политики
    /// </summary>
    public PoliticsContext() : base()
    {

    }

    /// <summary>
    /// Конструктор контекста бд политики с опциями
    /// </summary>
    /// <param cref="DbContextOptions{PoliticsContext}" name="options">Параметры</param>
    public PoliticsContext(DbContextOptions<PoliticsContext> options) : base(options)
    {

    }
    #endregion

    #region Поля
    /// <summary>
    /// Типы организаций
    /// </summary>
    public virtual DbSet<OrganizationType> OrganizationsTypes { get; set; }

    /// <summary>
    /// Организации
    /// </summary>
    public virtual DbSet<Organization> Organizations { get; set; }

    /// <summary>
    /// Страны
    /// </summary>
    public virtual DbSet<Country> Countries { get; set; }

    /// <summary>
    /// Типы координат
    /// </summary>
    public virtual DbSet<CoordinateTypePolitics> CoordinatesTypes { get; set; }

    /// <summary>
    /// Координаты
    /// </summary>
    public virtual DbSet<CoordinatePolitics> Coordinates { get; set; }

    /// <summary>
    /// Координаты стран
    /// </summary>
    public virtual DbSet<CountryCoordinate> CountriesCoordinates { get; set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод при создании моделей
    /// </summary>
    /// <param cref="ModelBuilder" name="modelBuilder">Конструктор моделей</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Установка схемы бд
        modelBuilder.HasDefaultSchema("insania_politics");

        //Проверка наличия расширения
        modelBuilder.HasPostgresExtension("postgis");

        //Смена базовой модели типа координаты
        modelBuilder.Ignore<CoordinateType>();
        modelBuilder.Entity<CoordinateTypePolitics>();

        //Смена базовой модели координаты
        modelBuilder.Ignore<Coordinate>();
        modelBuilder.Entity<CoordinatePolitics>();

        //Создание ограничения уникальности на псевдоним типа организации
        modelBuilder.Entity<OrganizationType>().HasAlternateKey(x => x.Alias);

        //Создание ограничения уникальности на псевдоним наименования организации
        modelBuilder.Entity<Organization>().HasAlternateKey(x => x.Name);

        //Создание ограничения уникальности на псевдоним страны
        modelBuilder.Entity<Country>().HasAlternateKey(x => x.Alias);
        
        //Создание ограничения уникальности на цвет страны на карте
        modelBuilder.Entity<Country>().HasAlternateKey(x => x.Color);

        //Создание ограничения уникальности на псевдоним типа координаты
        modelBuilder.Entity<CoordinateTypePolitics>().HasAlternateKey(x => x.Alias);

        //Добавление gin-индекса на поле с координатами
        modelBuilder.Entity<CoordinatePolitics>().HasIndex(x => x.PolygonEntity).HasMethod("gist");

        //Создание ограничения уникальности на координату страны
        modelBuilder.Entity<CountryCoordinate>().HasAlternateKey(x => new { x.CoordinateId, x.CountryId });
    }
    #endregion
}