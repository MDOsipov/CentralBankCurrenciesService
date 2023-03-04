using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CBCurrenciesService.Controllers;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.Pagination;
using FakeItEasy;
using FluentAssertions;
using HttpService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CBCurrenciesService.test.ControllerTest
{
	public class SbrInfoControllerTests
	{
		private readonly IHttpCbrService _httpCbrService;
		private readonly IMapper _mapper;
		private readonly ILoggerManager _logger;
		private readonly IMappingHelper _mappingHelper;
		private readonly SbrInfoController _sbrInfoController;

		public SbrInfoControllerTests()
		{
			_httpCbrService = A.Fake<IHttpCbrService>();
			_mapper = A.Fake<IMapper>();
			_logger = A.Fake<ILoggerManager>();
			_mappingHelper = A.Fake<IMappingHelper>();
			_sbrInfoController = new SbrInfoController(_httpCbrService, _mapper, _logger, _mappingHelper);
		}

		[Fact]
		public async void TaskController_GetCurrencies_ReturnsOkResult()
		{
			// Arrange
			var currencies = A.Fake<PagedList<SingleCurrencyData>>();
			var currenciesMapped = A.Fake<IEnumerable<SingleCurrencyDto>>();
			var singleCurrencyDataParameters = A.Fake<SingleCurrencyDataParameters>();
			var metadata = A.Fake<Metadata>();

			A.CallTo(() => _httpCbrService.GetCurrencies(singleCurrencyDataParameters)).Returns(currencies);
			A.CallTo(() => _mapper.Map<IEnumerable<SingleCurrencyDto>>(currencies)).Returns(currenciesMapped);
			A.CallTo(() => _mappingHelper.GetMetaData(currencies)).Returns(metadata);
			

			// Act
			var actionResult = await _sbrInfoController.GetCurrencies(singleCurrencyDataParameters);

			// Assert
			var okObjectResult = actionResult as OkObjectResult;
			Assert.IsType<OkObjectResult>(okObjectResult);
		}

		[Fact]
		public async void TaskController_GetCurrencies_ReturnsBadRequest()
		{
			// Arrange
			var currencies = A.Fake<PagedList<SingleCurrencyData>>();
			var currenciesMapped = A.Fake<IEnumerable<SingleCurrencyDto>>();
			SingleCurrencyDataParameters? singleCurrencyDataParameters = null;
			var metadata = A.Fake<Metadata>();

			A.CallTo(() => _httpCbrService.GetCurrencies(singleCurrencyDataParameters)).Returns(currencies);
			A.CallTo(() => _mapper.Map<IEnumerable<SingleCurrencyDto>>(currencies)).Returns(currenciesMapped);
			A.CallTo(() => _mappingHelper.GetMetaData(currencies)).Returns(metadata);


			// Act
			var actionResult = await _sbrInfoController.GetCurrencies(singleCurrencyDataParameters);

			// Assert
			var badRequestResult = actionResult as BadRequestObjectResult;
			Assert.IsType<BadRequestObjectResult>(badRequestResult);
		}

		[Fact]
		public async void TaskController_GetCurrencies_ReturnsRightItems()
		{
			// Arrange
			var currencies = A.Fake<PagedList<SingleCurrencyData>>();
			var currenciesMapped = A.Fake<IEnumerable<SingleCurrencyDto>>();
			var singleCurrencyDataParameters = A.Fake<SingleCurrencyDataParameters>();
			var metadata = A.Fake<Metadata>();

			A.CallTo(() => _httpCbrService.GetCurrencies(singleCurrencyDataParameters)).Returns(currencies);
			A.CallTo(() => _mapper.Map<IEnumerable<SingleCurrencyDto>>(currencies)).Returns(GetSampleData.SampleCurrencyDtoList);
			A.CallTo(() => _mappingHelper.GetMetaData(currencies)).Returns(metadata);

			// Act
			var actionResult = await _sbrInfoController.GetCurrencies(singleCurrencyDataParameters);

			// Assert
			var okObjectResult = actionResult as OkObjectResult;
			var content = okObjectResult.Value as IEnumerable<SingleCurrencyDto>;
			content.Should().BeAssignableTo<IEnumerable<SingleCurrencyDto>>();
			content.Should().HaveCount(GetSampleData.SampleCurrencyDtoList.Count());
		}

		[Fact]
		public async void TaskController_GetCurrency_ReturnsOkResult()
		{
			// Arrange
			string currencyId = "R01230";
			var currency = A.Fake<SingleCurrencyData>();
			var currencyMapped = A.Fake<SingleCurrencyDto>();

			A.CallTo(() => _httpCbrService.CurrencyExists(currencyId)).Returns(true);
			A.CallTo(() => _httpCbrService.GetCurrencyById(currencyId)).Returns(currency);
			A.CallTo(() => _mapper.Map<SingleCurrencyDto>(currency)).Returns(currencyMapped);

			// Act
			var actionResult = await _sbrInfoController.GetCurrency(currencyId);

			// Assert
			var okObjectResult = actionResult as OkObjectResult;
			Assert.IsType<OkObjectResult>(okObjectResult);
		}

		[Fact]
		public async void TaskController_GetCurrency_ReturnsBadRequest()
		{
			// Arrange
			string currencyId = "";
			var currency = A.Fake<SingleCurrencyData>();
			var currencyMapped = A.Fake<SingleCurrencyDto>();

			A.CallTo(() => _httpCbrService.CurrencyExists(currencyId)).Returns(true);
			A.CallTo(() => _httpCbrService.GetCurrencyById(currencyId)).Returns(currency);
			A.CallTo(() => _mapper.Map<SingleCurrencyDto>(currency)).Returns(currencyMapped);

			// Act
			var actionResult = await _sbrInfoController.GetCurrency(currencyId);

			// Assert
			var badRequestResult = actionResult as BadRequestResult;
			Assert.IsType<BadRequestResult>(badRequestResult);
		}

		[Fact]
		public async void TaskController_GetCurrency_ReturnsNotFound()
		{
			// Arrange
			string currencyId = "R01230ds";
			var currency = A.Fake<SingleCurrencyData>();
			var currencyMapped = A.Fake<SingleCurrencyDto>();

			A.CallTo(() => _httpCbrService.CurrencyExists(currencyId)).Returns(false);
			A.CallTo(() => _httpCbrService.GetCurrencyById(currencyId)).Returns(currency);
			A.CallTo(() => _mapper.Map<SingleCurrencyDto>(currency)).Returns(currencyMapped);

			// Act
			var actionResult = await _sbrInfoController.GetCurrency(currencyId);

			// Assert
			var notFoundResult = actionResult as NotFoundResult;
			Assert.IsType<NotFoundResult>(notFoundResult);
		}

		[Fact]
		public async void TaskController_GetCurrency_ReturnsRightItem()
		{
			// Arrange
			string currencyId = "R01230";
			var currency = A.Fake<SingleCurrencyData>();
			var currencyMapped = A.Fake<SingleCurrencyDto>();

			A.CallTo(() => _httpCbrService.CurrencyExists(currencyId)).Returns(true);
			A.CallTo(() => _httpCbrService.GetCurrencyById(currencyId)).Returns(currency);
			A.CallTo(() => _mapper.Map<SingleCurrencyDto>(currency)).Returns(GetSampleData.SampleCurrencyDto);

			// Act
			var actionResult = await _sbrInfoController.GetCurrency(currencyId);

			// Assert
			var okObjectResult = actionResult as OkObjectResult;
			var result = okObjectResult.Value as SingleCurrencyDto;

			Assert.IsType<SingleCurrencyDto>(result);
			result.Should().BeEquivalentTo(GetSampleData.SampleCurrencyDto);
		}

	}
}
