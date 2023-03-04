using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBCurrenciesService.test
{
	public static class GetSampleData
	{
		public static IEnumerable<SingleCurrencyDto> SampleCurrencyDtoList => new List<SingleCurrencyDto> 
		{
			new SingleCurrencyDto
			{
				ReportDate = DateTime.Now,
				Id = "R01235",
				NumCode = "840",
				CharCode = "USD",
				Nominal = 1,
				Name = "Доллар США",
				Value = 75.4592,
				Previous = 75.4729
			},
			new SingleCurrencyDto
			{
				ReportDate = DateTime.Now,
				Id = "R01239",
				NumCode = "978",
				CharCode = "EUR",
				Nominal = 1,
				Name = "Евро",
				Value = 80.0469,
				Previous = 80.1897
			},
			new SingleCurrencyDto
			{
				ReportDate = DateTime.Now,
				Id = "R01240",
				NumCode = "818",
				CharCode = "EGP",
				Nominal = 10,
				Name = "Египетских фунтов",
				Value = 24.5743,
				Previous = 24.6511
			}
		};

		public static SingleCurrencyDto SampleCurrencyDto => new SingleCurrencyDto()
		{
			ReportDate = DateTime.Parse("2023-03-04T11:30:00+03:00"),
			Id = "R01235",
			NumCode = "840",
			CharCode = "USD",
			Nominal = 1,
			Name = "Доллар США",
			Value = 75.4592,
			Previous = 75.4729
		};
	}
}

