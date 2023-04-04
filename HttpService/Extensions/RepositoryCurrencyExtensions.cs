using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace HttpService.Extensions
{
	public static class RepositoryCurrencyExtensions
	{
		public static IQueryable<SingleCurrencyData> SearchByName(this IQueryable<SingleCurrencyData> currencies, string? name)
		{
			if (!currencies.Any() || string.IsNullOrEmpty(name))
				return currencies;

			return currencies.Where(c => c.Name.ToLower().Contains(name.Trim().ToLower()));
		}

		public static IQueryable<SingleCurrencyData> ApplySort(this IQueryable<SingleCurrencyData> entities, string? orderByQueryString)
		{
			if (!entities.Any())
				return entities;

			if (string.IsNullOrWhiteSpace(orderByQueryString))
			{
				return entities;
			}

			string[] orderParams = orderByQueryString.Trim().Split(',');
			var propertyInfos = typeof(SingleCurrencyData).GetProperties(BindingFlags.Public | BindingFlags.Instance);
			var orderQueryBuilder = new StringBuilder();

			foreach (var param in orderParams)
			{
				if (string.IsNullOrWhiteSpace(param))
					continue;

				var propertyFromQueryName = param.Split(" ")[0];
				var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

				if (objectProperty == null)
					continue;

				var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";

				orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");

			}

			var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

			if (string.IsNullOrWhiteSpace(orderQuery))
			{
				return entities;
			}

			return entities.OrderBy(orderQuery);
		}
	}
}

