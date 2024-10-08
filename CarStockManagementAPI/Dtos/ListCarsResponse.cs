using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarStockManagementAPI.Dtos
{
    public class ListCarsResponse
    {
        public string Message { get; set; }
        public IEnumerable<CarResponse> Cars { get; set; } = new List<CarResponse>();
    }
    public class CarResponse
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public int Stock { get; set; }
    }
}