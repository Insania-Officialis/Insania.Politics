using Microsoft.Extensions.DependencyInjection;

using Insania.Politics.Contracts.BusinessLogic;
using Insania.Politics.DataAccess;

namespace Insania.Politics.BusinessLogic;

/// <summary>
/// Расширение для внедрения зависимостей сервисов работы с бизнес-логикой в зоне политики
/// </summary>
public static class Extension
{
    /// <summary>
    /// Метод внедрения зависимостей сервисов работы с бизнес-логикой в зоне политики
    /// </summary>
    /// <param cref="IServiceCollection" name="services">Исходная коллекция сервисов</param>
    /// <returns cref="IServiceCollection">Модифицированная коллекция сервисов</returns>
    public static IServiceCollection AddPoliticsBL(this IServiceCollection services) =>
        services
            .AddPoliticsDAO() //сервисы работы с данными в зоне политики
            .AddScoped<ICountriesBL, CountriesBL>() //сервис работы с бизнес-логикой стран
        ;
}