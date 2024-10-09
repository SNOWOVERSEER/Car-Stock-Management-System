using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarStockManagementAPI.Dtos
{
    public class LoginResponse
    {
        public string Message { get; set; }
        public string Token { get; set; }
    }
}