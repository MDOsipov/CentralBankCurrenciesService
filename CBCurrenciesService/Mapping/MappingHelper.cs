using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.Pagination;
using HttpService;
using System.Dynamic;

namespace CBCurrenciesService.Mapping
{
	public class MappingHelper : IMappingHelper
	{
		private readonly IHttpCbrService _httpCbrService;

		public MappingHelper(IHttpCbrService httpCbrService)
		{
			_httpCbrService = httpCbrService;
		}

		public void SetReportDate(IEnumerable<SingleCurrencyDto> currencies)
		{
			foreach(var currency in currencies) 
			{
				currency.ReportDate = _httpCbrService.LastUpdate;
			}
		}

		public void SetReportDate(IEnumerable<ExpandoObject> currencies)
		{
			foreach (var currency in currencies)
			{
				currency.TryAdd("ReportDate", _httpCbrService.LastUpdate);
			}
		}

		public void SetReportDate(SingleCurrencyDto currency)
		{
			currency.ReportDate = _httpCbrService.LastUpdate;
		}

		public void SetReportDate(ExpandoObject currency)
		{
			currency.TryAdd("ReportDate", _httpCbrService.LastUpdate);
		}

		public Metadata GetMetaData(PagedList<SingleCurrencyData> currencies)
		{
			return new Metadata()
			{
				TotalCount = currencies.TotalCount,
				PageSize = currencies.PageSize,
				CurrentPage = currencies.CurrentPage,
				TotalPages = currencies.TotalPages,
				HasNext = currencies.HasNext,
				HasPrevious = currencies.HasPrevious
			};
		}

		public Metadata GetMetaData(PagedList<ExpandoObject> currencies)
		{
			return new Metadata()
			{
				TotalCount = currencies.TotalCount,
				PageSize = currencies.PageSize,
				CurrentPage = currencies.CurrentPage,
				TotalPages = currencies.TotalPages,
				HasNext = currencies.HasNext,
				HasPrevious = currencies.HasPrevious
			};
		}
	}
}
