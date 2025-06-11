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
           {"LoggingOptions:FilePath", "E:\\Program\\Insania\\Logs\\Politics.Tests\\log.txt"},
           {"InitializationDataSettings:ScriptsPath", "E:\\Program\\Insania\\Insania.Politics\\Insania.Politics.Database\\Scripts"},
           {"InitializationDataSettings:InitStructure", "false"},
           {"InitializationDataSettings:Tables:OrganizationsTypes", "true"},
           {"InitializationDataSettings:Tables:Organizations", "true"},
           {"InitializationDataSettings:Tables:Countries", "true"},
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
        services.AddAutoMapper(typeof(PoliticsMappingProfile));

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
}