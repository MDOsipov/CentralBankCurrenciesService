using Entities.DataTransferObjects;
using Entities.Models;
using Entities.Pagination;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
	public interface IMappingHelper
	{
		void SetReportDate(IEnumerable<SingleCurrencyDto> currencies);
		void SetReportDate(IEnumerable<ExpandoObject> currencies);
		void SetReportDate(SingleCurrencyDto currency);
		void SetReportDate(ExpandoObject currency);
		Metadata GetMetaData(PagedList<SingleCurrencyData> currencies);
		Metadata GetMetaData(PagedList<ExpandoObject> currencies);

	}
}
