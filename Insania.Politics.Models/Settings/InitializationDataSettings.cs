namespace Insania.Politics.Models.Settings;

/// <summary>
/// Модель параметров инициализации данных
/// </summary>
public class InitializationDataSettings
{
    /// <summary>
    /// Признак инициализации структуры
    /// </summary>
    /// <remarks>
    /// Нужен для запуска миграций, при true не происходит инициализация данных
    /// </remarks>
    public bool? InitStructure { get; set; }

    /// <summary>
    /// Путь к файлам скриптов
    /// </summary>
    public string? ScriptsPath { get; set; }

    /// <summary>
    /// Включение в инициализацию таблиц
    /// </summary>
    public InitializationDataSettingsIncludeTables? Tables { get; set; }

    /// <summary>
    /// Включение в инициализацию баз данных
    /// </summary>
    public InitializationDataSettingsIncludeDatabases? Databases { get; set; }
}

/// <summary>
/// Модель параметра включения в инициализацию таблиц
/// </summary>
public class InitializationDataSettingsIncludeTables
{
    /// <summary>
    /// Типы организаций
    /// </summary>
    public bool? OrganizationsTypes { get; set; }

    /// <summary>
    /// Организации
    /// </summary>
    public bool? Organizations { get; set; }

    /// <summary>
    /// Страны
    /// </summary>
    public bool? Countries { get; set; }

    /// <summary>
    /// Типы координат
    /// </summary>
    public bool? CoordinatesTypes { get; set; }

    /// <summary>
    /// Координаты
    /// </summary>
    public bool? Coordinates { get; set; }

    /// <summary>
    /// Координаты стран
    /// </summary>
    public bool? CountriesCoordinates { get; set; }

    /// <summary>
    /// Регионы
    /// </summary>
    public bool? Regions { get; set; }

    /// <summary>
    /// Владения
    /// </summary>
    public bool? Domains { get; set; }

    /// <summary>
    /// Области
    /// </summary>
    public bool? Areas { get; set; }

    /// <summary>
    /// Уровень населённых пунктов
    /// </summary>
    public bool? LocalitiesLevels { get; set; }

    /// <summary>
    /// Населённые пункты
    /// </summary>
    public bool? Localities { get; set; }

    /// <summary>
    /// Параметры
    /// </summary>
    public bool? Parameters { get; set; }
}

/// <summary>
/// Модель параметра включения в инициализацию баз данных
/// </summary>
public class InitializationDataSettingsIncludeDatabases
{
    /// <summary>
    /// Политика
    /// </summary>
    public bool? Politics { get; set; }

    /// <summary>
    /// Логи сервиса политики
    /// </summary>
    public bool? LogsApiPolitics { get; set; }
}