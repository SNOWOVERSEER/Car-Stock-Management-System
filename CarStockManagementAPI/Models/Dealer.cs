using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarStockManagementAPI.Models
{
    public class Dealer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

    }
}