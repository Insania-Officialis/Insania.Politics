using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

using Insania.Shared.Contracts.DataAccess;
using Insania.Shared.Contracts.Services;
using Insania.Shared.Services;

using Insania.Politics.BusinessLogic;
using Insania.Politics.DataAccess;
using Insania.Politics.Database.Contexts;
using Insania.Politics.Models.Mapper;
using Insania.Politics.Models.Settings;

namespace Insania.Politics.Tests.Base;

/// <summary>
/// Базовый класс тестирования
/// </summary>
public abstract class BaseTest
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор базового класса тестирования
    /// </summary>
    public BaseTest()
    {
        //Создание коллекции сервисов
        IServiceCollection services = new ServiceCollection();

        //Создание коллекции ключей конфигурации
        Dictionary<string, string> configurationKeys = new()
        {
           {"LoggingOptions:FilePath", DetermineLogPath()},
           {"InitializationDataSettings:ScriptsPath", DetermineScriptsPath()},
           {"InitializationDataSettings:InitStructure", "false"},
           {"InitializationDataSettings:Tables:OrganizationsTypes", "true"},
           {"InitializationDataSettings:Tables:Organizations", "true"},
           {"InitializationDataSettings:Tables:Countries", "true"},
           {"InitializationDataSettings:Tables:CoordinatesTypes", "true"},
           {"InitializationDataSettings:Tables:Coordinates", "true"},
           {"InitializationDataSettings:Tables:CountriesCoordinates", "true"},
           {"InitializationDataSettings:Tables:Regions", "true"},
           {"InitializationDataSettings:Tables:Domains", "true"},
           {"InitializationDataSettings:Tables:Areas", "true"},
           {"InitializationDataSettings:Tables:LocalitiesLevels", "true"},
           {"InitializationDataSettings:Tables:Localities", "true"},
           {"TokenSettings:Issuer", "Politics.Test"},
           {"TokenSettings:Audience", "Politics.Test"},
           {"TokenSettings:Key", "This key is generated for tests in the user zone"},
           {"TokenSettings:Expires", "7"},
        };

        //Создание экземпляра конфигурации в памяти
        IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(configurationKeys!).Build();

        //Установка игнорирования типов даты и времени
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        //Внедрение зависимостей сервисов
        services.AddSingleton(_ => configuration); //конфигурация
        services.AddScoped<ITransliterationSL, TransliterationSL>(); //сервис транслитерации
        services.AddScoped<IPolygonParserSL, PolygonParserSL>(); //сервис преобразования полигонов
        services.AddScoped<IInitializationDAO, InitializationDAO>(); //сервис инициализации данных в бд политики
        services.AddPoliticsBL(); //сервисы работы с бизнес-логикой в зоне политики

        //Добавление контекстов бд в коллекцию сервисов
        services.AddDbContext<PoliticsContext>(options => options.UseInMemoryDatabase(databaseName: "insania_politics").ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))); //бд политики
        services.AddDbContext<LogsApiPoliticsContext>(options => options.UseInMemoryDatabase(databaseName: "insania_logs_api_politics").ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))); //бд логов сервиса политики

        //Добавление параметров логирования
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.File(path: configuration["LoggingOptions:FilePath"]!, rollingInterval: RollingInterval.Day)
            .WriteTo.Debug()
            .CreateLogger();
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger, dispose: true));

        //Добавление параметров преобразования моделей
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<PoliticsMappingProfile>();
        });

        //Добавление параметров инициализации данных
        IConfigurationSection? initializationDataSettings = configuration.GetSection("InitializationDataSettings");
        services.Configure<InitializationDataSettings>(initializationDataSettings);

        //Создание поставщика сервисов
        ServiceProvider = services.BuildServiceProvider();

        //Выполнение инициализации данных
        IInitializationDAO initialization = ServiceProvider.GetRequiredService<IInitializationDAO>();
        initialization.Initialize().Wait();
    }
    #endregion

    #region Поля
    /// <summary>
    /// Поставщик сервисов
    /// </summary>
    protected IServiceProvider ServiceProvider { get; set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод определения пути для логов
    /// </summary>
    /// <returns cref="string">Путь для сохранения логов</returns>
    private static string DetermineLogPath()
    {
        //Проверка запуска в докере
        bool isRunningInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true" || File.Exists("/.dockerenv");

        //Возврат нужного пути
        if (isRunningInDocker) return "/logs/log.txt";
        else return "G:\\Program\\Insania\\Logs\\Politics.Tests\\log.txt";
    }

    /// <summary>
    /// Метод определения пути для скриптов
    /// </summary>
    /// <returns cref="string">Путь к скриптам</returns>
    private static string DetermineScriptsPath()
    {
        //Проверка запуска в докере
        bool isRunningInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true" || File.Exists("/.dockerenv");

        if (isRunningInDocker) return "/src/Insania.Politics.Database/Scripts";
        else return "G:\\Program\\Insania\\Insania.Politics\\Insania.Politics.Database\\Scripts";
    }
    #endregion
}