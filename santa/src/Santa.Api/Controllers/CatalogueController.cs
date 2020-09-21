using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogueController : ControllerBase
    {
        private readonly IRepository repository;

        public CatalogueController(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }

        // POST: api/Catalogue/SearchClients
        [HttpPost("SearchClients")]
        public async Task<ActionResult<List<Client>>> GetClientsByQuery([FromBody] SearchQueries model)
        {
            try
            {
                List<Logic.Objects.Client> listLogicClient = await repository.SearchClientByQuery(model);

                return Ok(listLogicClient);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}