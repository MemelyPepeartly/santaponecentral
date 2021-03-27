using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;
using Santa.Logic.Objects.Information_Objects;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CatalogueController : ControllerBase
    {
        private readonly IRepository repository;

        public CatalogueController(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }

        // POST: api/Catalogue/SearchClients
        [HttpPost("SearchClients")]
        [Authorize(Policy = "read:clients")]
        public async Task<ActionResult<List<StrippedClient>>> GetClientsByQuery([FromBody] SearchQueries model)
        {
            List<StrippedClient> listLogicStrippedClients = await repository.SearchClientByQuery(model);

            return Ok(listLogicStrippedClients.OrderBy(c => c.nickname));
        }
    }
}