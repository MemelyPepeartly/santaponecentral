using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharkTank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YuleLogController : ControllerBase
    {
        // POST: api/<YuleLogController>/ManualLog
        /// <summary>
        /// Posts a manual log with the given body
        /// </summary>
        /// <returns></returns>
        [HttpPost("ManualLog")]
        public IEnumerable<string> Post()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
