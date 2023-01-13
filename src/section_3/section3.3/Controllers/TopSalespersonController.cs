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
            var res = await db.StringGetAsync(new RedisKey[] { "top:sales", "top:name" });
            long? topSales = (long?)res[0];
            string topSalesName = res[1];
            if(topSales.HasValue && !string.IsNullOrEmpty(topSalesName))
            {
                stopwatch.Stop();
                return Ok(new Dictionary<string, object>
                {
                    { "sum_sales", topSales },
                    { "employee_name", topSalesName },
                    { "time", stopwatch.ElapsedMilliseconds }
                });
            }

            var topSalesperson = await _salesContext.Employees
                .Select(x => new { Employee = x, sumSales = x.Sales
                .Sum(s => s.Total)})
                .OrderByDescending(a => a.sumSales)
                .FirstAsync();

            // add cache set logic here
            var topSalesSetTask = db.StringSetAsync("top:sales", topSalesperson.sumSales, expiry: TimeSpan.FromMinutes(5));
            var topNameSetTask = db.StringSetAsync("top:name", topSalesperson.Employee.Name, expiry: TimeSpan.FromMinutes(5));
            await Task.WhenAll(topSalesSetTask, topNameSetTask);
            

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
