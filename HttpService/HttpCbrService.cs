using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;
using System.Linq.Dynamic.Core;
using Entities.Helpers;

namespace HttpService
{
    public class HttpCbrService : IHttpCbrService
	{
		private readonly HttpClient _httpClient = new HttpClient();
		private readonly JsonSerializerOptions _options;
        private readonly IMemoryCache _cache;
		private readonly ISortHelper<SingleCurrencyData> _sortHelper;

		// private DailyCurrencyData _currentData;
		private DateTime _lastUpdate = DateTime.MinValue;
		public DateTime LastUpdate => _lastUpdate;
		
		public HttpCbrService(string httpBaseUri, IMemoryCache memoryCache, ISortHelper<SingleCurrencyData> sortHelper)
		{
			_httpClient.BaseAddress = new Uri(httpBaseUri);
			_httpClient.Timeout = new TimeSpan(0, 0, 30);
			_options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			_cache = memoryCache;
			_sortHelper = sortHelper;
		}

		public async Task<PagedList<SingleCurrencyData>> GetCurrencies(SingleCurrencyDataParameters singleCurrencyDataParameters)
		{
			DailyCurrencyData dailyCurrencyData = await GetCbrInfo();

			var currencies = dailyCurrencyData.Valute.Values.Where(c => c.Value >= singleCurrencyDataParameters.MinValue && c.Value <= singleCurrencyDataParameters.MaxValue).AsQueryable();

			SearchByName(ref currencies, singleCurrencyDataParameters.Name);
			currencies = _sortHelper.ApplySort(currencies, singleCurrencyDataParameters.OrderBy);

			return PagedList<SingleCurrencyData>.ToPagedList(currencies.ToList(),
								singleCurrencyDataParameters.PageNumber,
								singleCurrencyDataParameters.PageSize); ;
		}

		public async Task<SingleCurrencyData> GetCurrencyById (string currencyId)
		{
			DailyCurrencyData dailyCurrencyData = await GetCbrInfo();

			var currency = dailyCurrencyData.Valute.Values.Where(c => c.ID.Equals(currencyId)).SingleOrDefault();


			return currency;
		}

		public async Task<bool> CurrencyExists(string currencyId)
		{
			DailyCurrencyData dailyCurrencyData = await GetCbrInfo();

			var result = dailyCurrencyData.Valute.Values.Any(c => c.ID.Equals(currencyId));

			return result;
		}

		private async Task<DailyCurrencyData> GetCbrInfo()
		{
			DailyCurrencyData? dailyCurrencyData = null;	

			if (!_cache.TryGetValue("SBRInfo", out dailyCurrencyData))
			{
				var response = await _httpClient.GetAsync("");
				response.EnsureSuccessStatusCode();

				var content = await response.Content.ReadAsStringAsync();

				dailyCurrencyData = JsonSerializer.Deserialize<DailyCurrencyData>(content, _options);

				if (dailyCurrencyData != null)
				{
					_cache.Set("SBRInfo", dailyCurrencyData);
					_lastUpdate = DateTime.Now;
					return dailyCurrencyData;
                }

				throw new Exception("No data");
			}
			else if (ShouldUpdate() && _cache.TryGetValue("SBRInfo", out dailyCurrencyData))
			{
				_cache.Remove("SBRInfo");

                var response = await _httpClient.GetAsync("");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                dailyCurrencyData = JsonSerializer.Deserialize<DailyCurrencyData>(content, _options);

                if (dailyCurrencyData != null)
                {
                    _cache.Set("SBRInfo", dailyCurrencyData);
                    _lastUpdate = DateTime.Now;
                    return dailyCurrencyData;
                }

                throw new Exception("No data");
            }

            dailyCurrencyData = _cache.Get("SBRInfo") as DailyCurrencyData;
			return dailyCurrencyData;
        }

		private bool ShouldUpdate()
		{
			if (DateTime.Now.Day == _lastUpdate.Day && DateTime.Now.Month == _lastUpdate.Month && DateTime.Now.Year == _lastUpdate.Year)
				return false;

			if (DateTime.Now.Day != _lastUpdate.Day && (_lastUpdate.AddDays(1).Day == DateTime.Now.Day || _lastUpdate.AddDays(2).Day == DateTime.Now.Day))
			{
				if ((DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday) && _lastUpdate.DayOfWeek == DayOfWeek.Friday)
				{
					return false;
				}

				return true;
			}

			return true;
		}

		private void SearchByName(ref IQueryable<SingleCurrencyData> currencies, string? name)
		{
			if (!currencies.Any() || string.IsNullOrEmpty(name))
				return;

			currencies = currencies.Where(c => c.Name.ToLower().Contains(name.Trim().ToLower()));	
		}

	}
}
