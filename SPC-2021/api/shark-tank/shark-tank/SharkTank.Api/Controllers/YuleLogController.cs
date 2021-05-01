using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharkTank.Logic.Interfaces;
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
        private readonly IYuleLog yuleLogger;

        public YuleLogController(IYuleLog _yuleLogger)
        {
            yuleLogger = _yuleLogger ?? throw new ArgumentNullException(nameof(_yuleLogger));
        }

        // POST: api/<YuleLogController>/NewLog
        /// <summary>
        /// Posts a manual log with the given body
        /// </summary>
        /// <returns></returns>
        [HttpPost("NewLog")]
        public async Task<ActionResult<bool>> PostNewLog()
        {
            return StatusCode(StatusCodes.Status501NotImplemented, "We'll get there...");
        }
    }
}