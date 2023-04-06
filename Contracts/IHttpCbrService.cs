using System;
using Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Pagination;
using System.Dynamic;

namespace Contracts
{
    public interface IHttpCbrService
	{
		DateTime LastUpdate { get; } 
		Task<PagedList<SingleCurrencyData>> GetCurrencies(SingleCurrencyDataParameters singleCurrencyDataParameters);
		Task<PagedList<ExpandoObject>> GetCurrenciesShaped(SingleCurrencyDataParameters singleCurrencyDataParameters);
		Task<SingleCurrencyData> GetCurrencyById(string currencyId);
		Task<ExpandoObject> GetCurrencyByIdShaped(string currencyId, string? fields);
		Task<bool> CurrencyExists(string currencyId);
	}
}
