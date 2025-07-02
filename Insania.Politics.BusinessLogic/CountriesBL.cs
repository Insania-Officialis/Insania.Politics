using Microsoft.Extensions.Logging;

using AutoMapper;

using Insania.Shared.Models.Responses.Base;

using Insania.Politics.Contracts.BusinessLogic;
using Insania.Politics.Contracts.DataAccess;
using Insania.Politics.Entities;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;

using InformationMessages = Insania.Politics.Messages.InformationMessages;

namespace Insania.Politics.BusinessLogic;

/// <summary>
/// Сервис работы с бизнес-логикой стран
/// </summary>
/// <param cref="ILogger{CountriesBL}" name="logger">Сервис логгирования</param>
/// <param cref="IMapper" name="mapper">Сервис преобразования моделей</param>
/// <param cref="ICountriesDAO" name="countriesDAO">Сервис работы с данными стран</param>
public class CountriesBL(ILogger<CountriesBL> logger, IMapper mapper, ICountriesDAO countriesDAO) : ICountriesBL
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<CountriesBL> _logger = logger;

    /// <summary>
    /// Сервис преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Сервис работы с данными стран
    /// </summary>
    private readonly ICountriesDAO _countriesDAO = countriesDAO;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка стран
    /// </summary>
    /// <returns cref="BaseResponseList">Стандартный ответ</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<BaseResponseList> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListCountriesMethod);

            //Получение данных
            List<Country>? data = await _countriesDAO.GetList();

            //Формирование ответа
            BaseResponseList? response = null;
            if (data == null) response = new(false, null);
            else response = new(true, data?.Select(_mapper.Map<BaseResponseListItem>).ToList());

            //Возврат ответа
            return response;
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text}: {error}", ErrorMessages.Error, ex.Message);

            //Проброс исключения
            throw;
        }
    }
    #endregion
}