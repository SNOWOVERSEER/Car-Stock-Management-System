using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarStockManagementAPI.Dtos
{
    public class UpdateCarStockRequest
    {
        public int CarId { get; set; }
        public int NewStock { get; set; }
    }
}