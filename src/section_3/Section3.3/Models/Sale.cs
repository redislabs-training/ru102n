using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace section3._3.Models
{
    public class Sale
    {
        public int SaleId { get; set; }
        public int Total { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}