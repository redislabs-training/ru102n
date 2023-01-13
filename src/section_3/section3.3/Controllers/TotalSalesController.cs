using section3._3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Diagnostics;
using System.Data.Entity;

namespace section3._3.Controllers
{
    public class TotalSalesController : ApiController
    {
        private SalesContext _salesContext = new SalesContext();

        [ResponseType(typeof(Dictionary<string,long>))]
        public async Task<IHttpActionResult> GetTotalSales()
        {
            var stopwatch = Stopwatch.StartNew();

            // add cache check logic here
            var db = Redis.Database;
            long? totalSales = (long?) await db.StringGetAsync("totalSales");
            if(totalSales != null)
            {
                return Ok(new Dictionary<string, long>()
                {
                    { "Total Sales", totalSales.Value },
                    { "elapsed", stopwatch.ElapsedMilliseconds }
                });
            }
            
            totalSales = await _salesContext.Sales.SumAsync(x => x.Total);

            // add cache set logic here
            var timeTillMidnight = DateTime.Today.AddDays(1) - DateTime.Now;
            await db.StringSetAsync("totalSales", totalSales, timeTillMidnight);


            stopwatch.Stop();

            return Ok(new Dictionary<string, long>()
            {
                { "Total Sales", totalSales.Value },
                { "elapsed", stopwatch.ElapsedMilliseconds }
            });
        }
    }
}
