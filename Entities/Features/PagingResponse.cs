using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Features
{
	public class PagingResponse<T> where T : class
	{
		public List<T> Items { get; set; }
		public Metadata MetaData { get; set; }
	}
}
