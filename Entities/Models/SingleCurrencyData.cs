using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
	public class SingleCurrencyData
	{
		public string ID { get; set; }
		public string NumCode { get; set; }
		public string CharCode { get; set; }
		public int Nominal { get; set; }
		public string Name { get; set; }
		public double Value { get; set; }
		public double Previous { get; set; }
	}
}
