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

    /// <summary>
    /// Регионы
    /// </summary>
    public virtual DbSet<Region> Regions { get; set; }

    /// <summary>
    /// Владения
    /// </summary>
    public virtual DbSet<Domain> Domains { get; set; }

    /// <summary>
    /// Области
    /// </summary>
    public virtual DbSet<Area> Areas { get; set; }

    /// <summary>
    /// Уровни населённых пунктов
    /// </summary>
    public virtual DbSet<LocalityLevel> LocalitiesLevels { get; set; }

    /// <summary>
    /// Населённые пункты
    /// </summary>
    public virtual DbSet<Locality> Localities { get; set; }
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

        //Настройка сущности типа координаты
        modelBuilder.Entity<CoordinateType>(entity =>
        {
            //Установка наименования таблицы
            entity.ToTable("c_coordinates_types");

            //Смена базовой модели типа координаты
            entity.HasDiscriminator<string>("TypeDiscriminator")
                  .HasValue<CoordinateTypePolitics>("Politics");

            //Создание ограничения уникальности на псевдоним наименования типа координаты
            entity.HasAlternateKey(x => x.Alias);
        });

        //Настройка сущности координаты
        modelBuilder.Entity<Coordinate>(entity =>
        {
            //Установка наименования таблицы
            entity.ToTable("r_coordinates");

            //Смена базовой модели координаты
            entity.HasDiscriminator<string>("TypeDiscriminator")
                  .HasValue<CoordinatePolitics>("Politics");

            //Создание ограничения уникальности на псевдоним типа координаты
            modelBuilder.Entity<CoordinatePolitics>().HasIndex(x => x.PolygonEntity).HasMethod("gist");
        });

        //Создание ограничения уникальности на псевдоним типа организации
        modelBuilder.Entity<OrganizationType>().HasAlternateKey(x => x.Alias);

        //Создание ограничения уникальности на псевдоним наименования организации
        modelBuilder.Entity<Organization>().HasAlternateKey(x => x.Name);

        //Создание ограничения уникальности на псевдоним страны
        modelBuilder.Entity<Country>().HasAlternateKey(x => x.Alias);
        
        //Создание ограничения уникальности на цвет страны на карте
        modelBuilder.Entity<Country>().HasAlternateKey(x => x.Color);

        //Создание ограничения уникальности на координату страны
        modelBuilder.Entity<CountryCoordinate>().HasIndex(x => new { x.CoordinateId, x.CountryId, x.DateDeleted }).IsUnique();

        //Добавление вторичного ключа для координат
        modelBuilder.Entity<CountryCoordinate>()
            .HasOne(x => x.CoordinateEntity)
            .WithMany()
            .HasForeignKey(x => x.CoordinateId);

        //Создание ограничения уникальности на псевдоним региона
        modelBuilder.Entity<Region>().HasAlternateKey(x => x.Alias);

        //Создание ограничения уникальности на цвет региона на карте
        modelBuilder.Entity<Region>().HasAlternateKey(x => x.Color);

        //Создание ограничения уникальности на псевдоним владения
        modelBuilder.Entity<Domain>().HasAlternateKey(x => x.Alias);

        //Создание ограничения уникальности на цвет владения на карте
        modelBuilder.Entity<Domain>().HasAlternateKey(x => x.Color);

        //Создание ограничения уникальности на псевдоним области
        modelBuilder.Entity<Area>().HasAlternateKey(x => x.Alias);

        //Создание ограничения уникальности на цвет области на карте
        modelBuilder.Entity<Area>().HasAlternateKey(x => x.Color);

        //Создание ограничения уникальности на псевдоним населённого пункта
        modelBuilder.Entity<Locality>().HasAlternateKey(x => x.Alias);

        //Создание ограничения уникальности на псевдоним уровня населённого пункта
        modelBuilder.Entity<LocalityLevel>().HasAlternateKey(x => x.Alias);
    }
    #endregion
}