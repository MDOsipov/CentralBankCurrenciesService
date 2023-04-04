using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.Pagination;
using HttpService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CBCurrenciesService.Controllers
{
    [ApiController]
	[Route("")]
	public class SbrInfoController : ControllerBase
	{
		private readonly IHttpCbrService _httpCbrService;
		private readonly IMapper _mapper;
		private readonly ILoggerManager _logger;
		private readonly IMappingHelper _mappingHelper;

		public SbrInfoController(IHttpCbrService httpCbrService, IMapper mapper, ILoggerManager logger, IMappingHelper mappingHelper)
		{
			_httpCbrService = httpCbrService;
			_mapper = mapper;
			_logger = logger;
			_mappingHelper = mappingHelper;
		}

		[HttpGet("currencies")]
		public async Task<IActionResult> GetCurrencies([FromQuery] SingleCurrencyDataParameters singleCurrencyDataParameters)
		{
			_logger.LogDebug($"GetCurrencies method started");

			if (!singleCurrencyDataParameters.ValidValueRange)
				return BadRequest("Max value can not be less than min value");

			if (singleCurrencyDataParameters is null)
			{
				_logger.LogError("SingleCurrencyDataParameters object sent from client is null");
				return BadRequest("SingleCurrencyDataParameters object is null");
			}

			if (singleCurrencyDataParameters.PageNumber < 1 || singleCurrencyDataParameters.PageSize < 0)
			{
				_logger.LogError("Pagination parameters sent from client are not valid");
				return BadRequest("Pagination parameters are not valid");
			}

			if (!ModelState.IsValid)
			{
				_logger.LogError("SingleCurrencyDataParameters object sent from client is invalid");
				return BadRequest("SingleCurrencyDataParameters object is invalid");	
			}

			try
			{
				var currencies = await _httpCbrService.GetCurrencies(singleCurrencyDataParameters);

				Metadata metadata = _mappingHelper.GetMetaData(currencies);

				Response?.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));		
				_logger.LogInfo($"Success. Currencies are received");

				var currenciesMapped = _mapper.Map<IEnumerable<SingleCurrencyDto>>(currencies);

				_mappingHelper.SetReportDate(currenciesMapped);

				return Ok(currenciesMapped);
			}
			catch (HttpRequestException ex)
			{
				_logger.LogError($"Http request exception. Something went wrong while requesting Central Bank API");
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
			catch (Exception ex) 
			{
				_logger.LogError($"Internal server error. Something went wrong inside GetCurrencies action: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("currency/{currencyId}")]
		public async Task<IActionResult> GetCurrency(string currencyId)
		{
			_logger.LogDebug($"GetCurrency method started with parameter currencyId: {currencyId}");

			if (string.IsNullOrEmpty(currencyId))
			{
				_logger.LogError($"Bad request. Argument currencyId is invalid: {currencyId}");
				return BadRequest();
			}

			if (!await _httpCbrService.CurrencyExists(currencyId))
			{
				_logger.LogError($"Not found. Currency with id of {currencyId} hasn't been found");
				return NotFound();
			}

			try
			{
				var currency = await _httpCbrService.GetCurrencyById(currencyId);
				var currencyMapped = _mapper.Map<SingleCurrencyDto>(currency);
				_mappingHelper.SetReportDate(currencyMapped);

				_logger.LogInfo("Success. Currency is received");
				return Ok(currencyMapped);
			}
			catch (HttpRequestException ex)
			{
				_logger.LogError($"Http request exception. Something went wrong while requesting Central Bank API");
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Internal server error. Something went wrong inside GetCurrency action: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

	}
}
