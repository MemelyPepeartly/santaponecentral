using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Santa.Api.Services.YuleLog;
using Santa.Logic.Objects.Base_Objects.Logging;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LogController : ControllerBase
    {
        private readonly IYuleLog yuleLog;
        public LogController(IYuleLog _yuleLog)
        {
            yuleLog = _yuleLog ?? throw new ArgumentNullException(nameof(_yuleLog));

        }

        // GET: api/Log
        /// <summary>
        /// Endpoint returns a list of all logs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "read:logs")]
        public async Task<ActionResult<List<Logic.Objects.Base_Objects.Logging.YuleLog>>> GetAllLogs()
        {
            return Ok(new List<Logic.Objects.Base_Objects.Logging.YuleLog>());
        }

        // GET: api/Log/Category/5
        /// <summary>
        /// Gets a list of logs by a specific category ID
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        [HttpGet("Category/{categoryID}")]
        [Authorize(Policy = "read:logs")]
        public async Task<ActionResult<List<Logic.Objects.Base_Objects.Logging.YuleLog>>> GetAllLogsByCategoryID(Guid categoryID)
        {
            return Ok(new List<Logic.Objects.Base_Objects.Logging.YuleLog>());
        }
    }
}
