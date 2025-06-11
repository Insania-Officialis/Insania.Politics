using AutoMapper;

using Insania.Shared.Models.Responses.Base;

using Insania.Politics.Entities;

namespace Insania.Politics.Models.Mapper;

/// <summary>
/// Сервис преобразования моделей
/// </summary>
public class PoliticsMappingProfile : Profile
{
    /// <summary>
    /// Конструктор сервиса преобразования моделей
    /// </summary>
    public PoliticsMappingProfile()
    {
        //Преобразование модели сущности страны в базовую модель элемента ответа списком
        CreateMap<Country, BaseResponseListItem>();
    }
}