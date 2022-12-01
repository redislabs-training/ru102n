using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace section3._3.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public List<Sale> Sales { get; } = new List<Sale>();
    }
}