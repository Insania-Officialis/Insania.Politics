using System.Text.RegularExpressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Npgsql;

using Insania.Shared.Contracts.DataAccess;
using Insania.Shared.Contracts.Services;

using Insania.Politics.Database.Contexts;
using Insania.Politics.Entities;
using Insania.Politics.Models.Settings;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;
using InformationMessages = Insania.Shared.Messages.InformationMessages;

using ErrorMessagesPolitics = Insania.Politics.Messages.ErrorMessages;

namespace Insania.Politics.DataAccess;

/// <summary>
/// Сервис инициализации данных в бд политики
/// </summary>
/// <param cref="ILogger{InitializationDAO}" name="logger">Сервис логгирования</param>
/// <param cref="PoliticsContext" name="politicsContext">Контекст базы данных политики</param>
/// <param cref="LogsApiPoliticsContext" name="logsApiPoliticsContext">Контекст базы данных логов сервиса политики</param>
/// <param cref="IOptions{InitializationDataSettings}" name="settings">Параметры инициализации данных</param>
/// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
/// <param cref="IConfiguration" name="configuration">Конфигурация приложения</param>
public class InitializationDAO(ILogger<InitializationDAO> logger, PoliticsContext politicsContext, LogsApiPoliticsContext logsApiPoliticsContext, IOptions<InitializationDataSettings> settings, ITransliterationSL transliteration, IConfiguration configuration) : IInitializationDAO
{
    #region Поля
    private readonly string _username = "initializer";
    #endregion

    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<InitializationDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных политики
    /// </summary>
    private readonly PoliticsContext _politicsContext = politicsContext;

    /// <summary>
    /// Контекст базы данных логов сервиса политики
    /// </summary>
    private readonly LogsApiPoliticsContext _logsApiPoliticsContext = logsApiPoliticsContext;

    /// <summary>
    /// Параметры инициализации данных
    /// </summary>
    private readonly IOptions<InitializationDataSettings> _settings = settings;

    /// <summary>
    /// Сервис транслитерации
    /// </summary>
    private readonly ITransliterationSL _transliteration = transliteration;

    /// <summary>
    /// Конфигурация приложения
    /// </summary>
    private readonly IConfiguration _configuration = configuration;
    #endregion

    #region Методы
    /// <summary>
    /// Метод инициализации данных
    /// </summary>
    /// <exception cref="Exception">Исключение</exception>
    public async Task Initialize()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredInitializeMethod);

            //Инициализация структуры
            if (_settings.Value.InitStructure == true)
            {
                //Логгирование
                _logger.LogInformation("{text}", InformationMessages.InitializationStructure);

                //Инициализация баз данных в зависимости от параметров
                if (_settings.Value.Databases?.Politics == true)
                {
                    //Формирование параметров
                    string connectionServer = _configuration.GetConnectionString("PoliticsSever") ?? throw new Exception(ErrorMessagesShared.EmptyConnectionString);
                    string patternDatabases = @"^databases_politics_\d+\.sql$";
                    string connectionDatabase = _configuration.GetConnectionString("PoliticsEmpty") ?? throw new Exception(ErrorMessagesShared.EmptyConnectionString);
                    string patternSchemes = @"^schemes_politics_\d+\.sql$";
                    string patternExtension = @"^extension_politics_\d+\.sql$";

                    //Создание базы данных
                    await CreateDatabase(connectionServer, patternDatabases, connectionDatabase, patternSchemes, patternExtension);
                }
                if (_settings.Value.Databases?.LogsApiPolitics == true)
                {
                    //Формирование параметров
                    string connectionServer = _configuration.GetConnectionString("LogsApiPoliticsServer") ?? throw new Exception(ErrorMessagesShared.EmptyConnectionString);
                    string patternDatabases = @"^databases_logs_api_politics_\d+\.sql$";
                    string connectionDatabase = _configuration.GetConnectionString("LogsApiPoliticsEmpty") ?? throw new Exception(ErrorMessagesShared.EmptyConnectionString);
                    string patternSchemes = @"^schemes_logs_api_politics_\d+\.sql$";

                    //Создание базы данных
                    await CreateDatabase(connectionServer, patternDatabases, connectionDatabase, patternSchemes);
                }

                //Выход
                return;
            }

            //Накат миграций
            if (_politicsContext.Database.IsRelational()) await _politicsContext.Database.MigrateAsync();
            if (_logsApiPoliticsContext.Database.IsRelational()) await _logsApiPoliticsContext.Database.MigrateAsync();

            //Проверки
            if (string.IsNullOrWhiteSpace(_settings.Value.ScriptsPath)) throw new Exception(ErrorMessagesShared.EmptyScriptsPath);

            //Инициализация данных в зависимости от параметров
            if (_settings.Value.Tables?.OrganizationsTypes == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _politicsContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции сущностей
                    List<OrganizationType> entities =
                    [
                        new(_transliteration, 1, _username, "Удалённый", DateTime.UtcNow),
                        new(_transliteration, 2, _username, "Страны", null),
                    ];

                    //Проход по коллекции сущностей
                    foreach (var entity in entities)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_politicsContext.OrganizationsTypes.Any(x => x.Id == entity.Id)) await _politicsContext.OrganizationsTypes.AddAsync(entity);
                    }

                    //Сохранение изменений в бд
                    await _politicsContext.SaveChangesAsync();

                    //Создание шаблона файла скриптов
                    string pattern = @"^t_organizations_types_\d+.sql";

                    //Проходим по всем скриптам
                    foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
                    {
                        //Выполняем скрипт
                        await ExecuteScript(file, _politicsContext);
                    }

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
            if (_settings.Value.Tables?.Organizations == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _politicsContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции ключей
                    string[][] keys =
                    [
                        ["1", "Альвраатская империя", "2", "", ""],
                        ["2", "Княжество Саорса", "2", "", ""],
                        ["3", "Королевство Берген", "2", "", ""],
                        ["4", "Фесгарское княжество", "2", "", ""],
                        ["5", "Сверденский каганат", "2", "", ""],
                        ["6", "Ханство Тавалин", "2", "", ""],
                        ["7", "Княжество Саргиб", "2", "", ""],
                        ["8", "Царство Банду", "2", "", ""],
                        ["9", "Королевство Нордер", "2", "", ""],
                        ["10", "Альтерское княжество", "2", "", ""],
                        ["11", "Орлиадарская конфедерация", "2", "", ""],
                        ["12", "Королевство Удстир", "2", "", ""],
                        ["13", "Королевство Вервирунг", "2", "", ""],
                        ["14", "Дестинский орден", "2", "", ""],
                        ["15", "Вольный город Лийсет", "2", "", ""],
                        ["16", "Лисцийская империя", "2", "", ""],
                        ["17", "Королевство Вальтир", "2", "", ""],
                        ["18", "Вассальное княжество Гратис", "2", "", ""],
                        ["19", "Княжество Ректа", "2", "", ""],
                        ["20", "Волар", "2", "", ""],
                        ["21", "Союз Иль-Ладро", "2", "", ""],
                        ["22", "Мергерская Уния", "2", "", ""],
                        ["10000", "Удалённая", "1", "", DateTime.UtcNow.ToString()],
                    ];

                    //Проход по коллекции ключей
                    foreach (var key in keys)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_politicsContext.Organizations.Any(x => x.Id == long.Parse(key[0])))
                        {
                            //Получение сущностей
                            OrganizationType type = await _politicsContext.OrganizationsTypes.FirstOrDefaultAsync(x => x.Id == long.Parse(key[2])) ?? throw new Exception(ErrorMessagesPolitics.NotFoundOrganizationType);
                            Organization? parent = !string.IsNullOrWhiteSpace(key[3])
                                ? (await _politicsContext.Organizations.FirstOrDefaultAsync(x => x.Id == long.Parse(key[3])) ?? throw new Exception(ErrorMessagesPolitics.NotFoundOrganization))
                                : null;

                            //Создание сущности
                            DateTime? dateDeleted = null;
                            if (!string.IsNullOrWhiteSpace(key[4])) dateDeleted = DateTime.Parse(key[4]);
                            Organization entity = new(long.Parse(key[0]), _username, key[1], true, type, parent, dateDeleted);

                            //Добавление сущности в бд
                            await _politicsContext.Organizations.AddAsync(entity);
                        }
                    }

                    //Создание шаблона файла скриптов
                    string pattern = @"^t_organizations_\d+.sql";

                    //Проходим по всем скриптам
                    foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
                    {
                        //Выполняем скрипт
                        await ExecuteScript(file, _politicsContext);
                    }

                    //Сохранение изменений в бд
                    await _politicsContext.SaveChangesAsync();

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
            if (_settings.Value.Tables?.Countries == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _politicsContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции ключей
                    string[][] keys =
                    [
                        ["1", "Альвраатская империя", "Альвраатская империя - ", "Исландский", "#20D1DB", "1", ""],
                        ["2", "Княжество Саорса", "Княжество Саорса - ", "Ирландский", "#607F47", "2", ""],
                        ["3", "Королевство Берген", "Королевство Берген - ", "Шведский", "#00687C", "3", ""],
                        ["4", "Фесгарское княжество", "Фесгарское княжество - ", "Шотландский", "#B200FF", "4", ""],
                        ["5", "Сверденский каганат", "Сверденский каганат - ", "Норвежский", "#7F3B00", "5", ""],
                        ["6", "Ханство Тавалин", "Ханство Тавалин - ", "Эстонский", "#7F006D", "6", ""],
                        ["7", "Княжество Саргиб", "Княжество Саргиб - ", "Литовский", "#007F0E", "7", ""],
                        ["8", "Царство Банду", "Царство Банду - ", "Хинди", "#47617C", "8", ""],
                        ["9", "Королевство Нордер", "Королевство Нордер - ", "Немецкий", "#D82929", "9", ""],
                        ["10", "Альтерское княжество", "Альтерское княжество - ", "Французский", "#4ACC39", "10", ""],
                        ["11", "Орлиадарская конфедерация", "Орлиадарская конфедерация - ", "Французский", "#AF9200", "11", ""],
                        ["12", "Королевство Удстир", "Королевство Удстир - ", "Датский", "#8CAF00", "12", ""],
                        ["13", "Королевство Вервирунг", "Королевство Вервирунг - ", "Немецкий", "#7F1700", "13", ""],
                        ["14", "Дестинский орден", "Дестинский орден - ", "Итальянский", "#2B7C55", "14", ""],
                        ["15", "Вольный город Лийсет", "Вольный город Лийсет - ", "Итальянский", "#7B7F00", "15", ""],
                        ["16", "Лисцийская империя", "Лисцийская империя - ", "Итальянский", "#7F002E", "16", ""],
                        ["17", "Королевство Вальтир", "Королевство Вальтир - ", "Норвежский", "#B05BFF", "17", ""],
                        ["18", "Вассальное княжество Гратис", "Вассальное княжество Гратис - ", "Итальянский", "#005DFF", "18", ""],
                        ["19", "Княжество Ректа", "Княжество Ректа - ", "Эсперанто", "#487F00", "19", ""],
                        ["20", "Волар", "Волар - ", "Эсперанто", "#32217A", "20", ""],
                        ["21", "Союз Иль-Ладро", "Союз Иль-Ладро - ", "Итальянский", "#35513B", "21", ""],
                        ["22", "Мергерская Уния", "Мергерская Уния - ", "Латынь", "#BC3CB4", "22", ""],
                        ["10000", "Удалённая", "", "", "", "10000", DateTime.UtcNow.ToString()]
                    ];

                    //Проход по коллекции ключей
                    foreach (var key in keys)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_politicsContext.Countries.Any(x => x.Id == long.Parse(key[0])))
                        {
                            //Получение сущностей
                            Organization organization = await _politicsContext.Organizations.FirstOrDefaultAsync(x => x.Id == long.Parse(key[5])) ?? throw new Exception(ErrorMessagesPolitics.NotFoundOrganization);

                            //Создание сущности
                            DateTime? dateDeleted = null;
                            if (!string.IsNullOrWhiteSpace(key[6])) dateDeleted = DateTime.Parse(key[6]);
                            Country entity = new(_transliteration, long.Parse(key[0]), _username, key[1], key[2], key[3], key[4], organization, dateDeleted);

                            //Добавление сущности в бд
                            await _politicsContext.Countries.AddAsync(entity);
                        }
                    }

                    //Создание шаблона файла скриптов
                    string pattern = @"^t_countries_\d+.sql";

                    //Проходим по всем скриптам
                    foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
                    {
                        //Выполняем скрипт
                        await ExecuteScript(file, _politicsContext);
                    }

                    //Сохранение изменений в бд
                    await _politicsContext.SaveChangesAsync();

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
            if (_settings.Value.Tables?.CoordinatesTypes == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _politicsContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции сущностей
                    List<CoordinateTypePolitics> entities =
                    [
                        new(_transliteration, 1, _username, "Удалённый", DateTime.UtcNow),
                        new(_transliteration, 2, _username, "Страны", null),
                    ];

                    //Проход по коллекции сущностей
                    foreach (var entity in entities)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_politicsContext.CoordinatesTypes.Any(x => x.Id == entity.Id)) await _politicsContext.CoordinatesTypes.AddAsync(entity);
                    }

                    //Сохранение изменений в бд
                    await _politicsContext.SaveChangesAsync();

                    //Создание шаблона файла скриптов
                    string pattern = @"^t_coordinates_types_\d+.sql";

                    //Проходим по всем скриптам
                    foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
                    {
                        //Выполняем скрипт
                        await ExecuteScript(file, _politicsContext);
                    }

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text}: {error}", ErrorMessagesShared.Error, ex.Message);

            //Проброс исключения
            throw;
        }
    }

    /// <summary>
    /// Метод создание базы данных
    /// </summary>
    /// <param cref="string" name="connectionServer">Строка подключения к серверу</param>
    /// <param cref="string" name="patternDatabases">Шаблон файлов создания базы данных</param>
    /// <param cref="string" name="connectionDatabase">Строка подключения к базе данных</param>
    /// <param cref="string" name="patternSchemes">Шаблон файлов создания схемы</param>
    /// <param cref="string?" name="patternExtension">Шаблон файлов создания расширений</param>
    /// <returns></returns>
    private async Task CreateDatabase(string connectionServer, string patternDatabases, string connectionDatabase, string patternSchemes, string? patternExtension = null)
    {
        //Проход по всем скриптам в директории и создание баз данных
        foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), patternDatabases)))
        {
            //Выполнение скрипта
            await ExecuteScript(file, connectionServer);
        }

        //Проход по всем скриптам в директории и создание схем
        foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), patternSchemes)))
        {
            //Выполнение скрипта
            await ExecuteScript(file, connectionDatabase);
        }

        //Проход по всем скриптам в директории и создание расширений
        if (!string.IsNullOrWhiteSpace(patternExtension))
        {
            foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), patternExtension)))
            {
                //Выполнение скрипта
                await ExecuteScript(file, connectionDatabase);
            }
        }
    }

    /// <summary>
    /// Метод выполнения скрипта со строкой подключения
    /// </summary>
    /// <param cref="string" name="filePath">Путь к скрипту</param>
    /// <param cref="string" name="connectionString">Строка подключения</param>
    private async Task ExecuteScript(string filePath, string connectionString)
    {
        //Логгирование
        _logger.LogInformation("{text} {params}", InformationMessages.ExecuteScript, filePath);

        try
        {
            //Создание соединения к бд
            using NpgsqlConnection connection = new(connectionString);

            //Открытие соединения
            connection.Open();

            //Считывание запроса
            string sql = File.ReadAllText(filePath);

            //Создание sql-запроса
            using NpgsqlCommand command = new(sql, connection);

            //Выполнение команды
            await command.ExecuteNonQueryAsync();

            //Логгирование
            _logger.LogInformation("{text} {params}", InformationMessages.ExecutedScript, filePath);
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text} {params} из-за ошибки {ex}", ErrorMessagesShared.NotExecutedScript, filePath, ex);
        }
    }

    /// <summary>
    /// Метод выполнения скрипта с контекстом
    /// </summary>
    /// <param cref="string" name="filePath">Путь к скрипту</param>
    /// <param cref="DbContext" name="context">Контекст базы данных</param>
    private async Task ExecuteScript(string filePath, DbContext context)
    {
        //Логгирование
        _logger.LogInformation("{text} {params}", InformationMessages.ExecuteScript, filePath);

        try
        {
            //Считывание запроса
            string sql = File.ReadAllText(filePath);

            //Выполнение sql-команды
            await context.Database.ExecuteSqlRawAsync(sql);

            //Логгирование
            _logger.LogInformation("{text} {params}", InformationMessages.ExecutedScript, filePath);
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text} {params} из-за ошибки {ex}", ErrorMessagesShared.NotExecutedScript, filePath, ex);
        }
    }
    #endregion
}