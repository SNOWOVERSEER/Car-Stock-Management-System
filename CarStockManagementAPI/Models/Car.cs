using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarStockManagementAPI.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public int Stock { get; set; }
        public int DealerId { get; set; }


    }
}