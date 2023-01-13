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
    public class AverageController : ApiController
    {
        private SalesContext _salesContext = new SalesContext();


        [ResponseType(typeof(Dictionary<string,double>))]
        public async Task<IHttpActionResult> GetAverage(int id)
        {
            var stopwatch = Stopwatch.StartNew();

            // add cache check here
            var db = Redis.Database;
            var avg = (double?)await db.StringGetSetExpiryAsync($"average:{id}", TimeSpan.FromHours(1));
            if(avg != null)
            {
                stopwatch.Stop();
                return Ok(new Dictionary<string, double>
                {
                    {"average", avg.Value },
                    {"elapsed", stopwatch.ElapsedMilliseconds }
                });
            }

            avg = await _salesContext.Employees.Include("Sales").Where(x => x.EmployeeId == id).Select(x => x.Sales.Average(s => s.Total)).FirstAsync();
            // add cache set here
            await db.StringSetAsync($"average:{id}", avg, TimeSpan.FromHours(1));

            return Ok(new Dictionary<string, double>
            {
                {"average", avg.Value },
                {"elapsed", stopwatch.ElapsedMilliseconds }
            });
        }
    }
}
