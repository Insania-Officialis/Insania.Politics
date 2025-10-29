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
            .AddScoped<ICoordinatesTypesDAO, CoordinatesTypesDAO>() //сервис работы с данными типов координат
            .AddScoped<ICoordinatesDAO, CoordinatesDAO>() //сервис работы с данными координат
            .AddScoped<ICountriesCoordinatesDAO, CountriesCoordinatesDAO>() //сервис работы с данными координат стран
            .AddScoped<IRegionsDAO, RegionsDAO>() //сервис работы с данными регионов
            .AddScoped<IDomainsDAO, DomainsDAO>() //сервис работы с данными владений
            .AddScoped<IAreasDAO, AreasDAO>() //сервис работы с данными областей
            .AddScoped<ILocalitiesLevelsDAO, LocalitiesLevelsDAO>() //сервис работы с данными уровней населённых пунктов
            .AddScoped<ILocalitiesDAO, LocalitiesDAO>() //сервис работы с данными населённых пунктов
            .AddScoped<IParametersDAO, ParametersDAO>() //сервис работы с данными параметров
        ;
}