using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarStockManagementAPI.Dtos
{
    public class SearchCarResponse
    {
        public string Message { get; set; }
        public IEnumerable<CarResponse> Cars { get; set; } = new List<CarResponse>();

    }
}