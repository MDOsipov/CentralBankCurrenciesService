using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
	public class DailyCurrencyData
	{
		public DateTime Date { get; set; }
		public DateTime PreviousDate { get; set; }
		public string PreviousURL { get; set; }
		public DateTime TimeStamp { get; set; }
		public Dictionary<string, SingleCurrencyData> Valute { get; set; }
	}
}
