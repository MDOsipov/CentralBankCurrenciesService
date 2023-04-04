using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Pagination
{
    public class SingleCurrencyDataParameters
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 15;
        public uint MinValue { get; set; }
        public uint MaxValue { get; set; } = int.MaxValue;
        public bool ValidValueRange => MinValue <= MaxValue;    
        public string? Name { get; set; }

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value > maxPageSize ? maxPageSize : value;
            }
        }
    }
}
