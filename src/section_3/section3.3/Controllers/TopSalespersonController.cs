using section3._3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Diagnostics;
using StackExchange.Redis;
using System.Data.Entity.Spatial;

namespace section3._3.Controllers
{
    public class TopSalespersonController : ApiController
    {
        private SalesContext _salesContext = new SalesContext();

        [ResponseType(typeof(Dictionary<string, object>))]
        public async Task<IHttpActionResult> GetTopSalesperson()
        {
            var stopwatch = Stopwatch.StartNew();

            // add cache check here
            var db = Redis.Database;
            // TODO Section 3.3 step 3

            // end Section 3.3 step 3

            var topSalesperson = await _salesContext.Employees
                .Select(x => new { Employee = x, sumSales = x.Sales
                .Sum(s => s.Total)})
                .OrderByDescending(a => a.sumSales)
                .FirstAsync();

            // TODO Section 3.3 step 4
            // add cache set logic here

            // End Section 3.3 step 4

            stopwatch.Stop();
            return Ok(new Dictionary<string, object>
            {
                { "sum_sales", topSalesperson.sumSales },
                { "employee_name", topSalesperson.Employee.Name },
                { "time", stopwatch.ElapsedMilliseconds}
            });
        }
    }
}
