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

            // TODO Section 3.3 step 5
            // add cache check logic here
            var db = Redis.Database;

            // end Section 3.3 step 5
            
            totalSales = await _salesContext.Sales.SumAsync(x => x.Total);

            // TODO Section 3.3 step 6
            // add cache set logic here

            // end section 3.3 step 6

            stopwatch.Stop();

            return Ok(new Dictionary<string, long>()
            {
                { "Total Sales", totalSales.Value },
                { "elapsed", stopwatch.ElapsedMilliseconds }
            });
        }
    }
}
