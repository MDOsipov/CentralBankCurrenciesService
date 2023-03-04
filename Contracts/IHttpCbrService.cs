using System;
using Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Pagination;

namespace Contracts
{
    public interface IHttpCbrService
	{
		DateTime LastUpdate { get; } 
		Task<PagedList<SingleCurrencyData>> GetCurrencies(SingleCurrencyDataParameters singleCurrencyDataParameters);
		Task<SingleCurrencyData> GetCurrencyById(string currencyId);
		Task<bool> CurrencyExists(string currencyId);
	}
}
