using Entities.DataTransferObjects;
using Entities.Models;
using Entities.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
	public interface IMappingHelper
	{
		void SetReportDate(IEnumerable<SingleCurrencyDto> currencies);
		void SetReportDate(SingleCurrencyDto currency);
		Metadata GetMetaData(PagedList<SingleCurrencyData> currencies);
	}
}
