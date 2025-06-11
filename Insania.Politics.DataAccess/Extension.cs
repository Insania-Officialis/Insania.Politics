using Microsoft.Extensions.DependencyInjection;

using Insania.Politics.Contracts.DataAccess;

namespace Insania.Politics.DataAccess;

/// <summary>
/// Расширение для внедрения зависимостей сервисов работы с данными в зоне политики
/// </summary>
public static class Extension
{
    /// <summary>
    /// Метод внедрения зависимостей сервисов работы с данными в зоне политики
    /// </summary>
    /// <param cref="IServiceCollection" name="services">Исходная коллекция сервисов</param>
    /// <returns cref="IServiceCollection">Модифицированная коллекция сервисов</returns>
    public static IServiceCollection AddPoliticsDAO(this IServiceCollection services) =>
        services
            .AddScoped<IOrganizationsTypesDAO, OrganizationsTypesDAO>() //сервис работы с данными типов организаций
            .AddScoped<IOrganizationsDAO, OrganizationsDAO>() //сервис работы с данными организаций
            .AddScoped<ICountriesDAO, CountriesDAO>() //сервис работы с данными стран
        ;
}