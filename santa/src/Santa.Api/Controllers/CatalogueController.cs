using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Santa.Api.Models.Catalogue_Models;
using Santa.Api.Services.Searcher_Service;
using Santa.Data.Entities;
using Santa.Logic.Interfaces;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogueController : ControllerBase
    {
        private readonly ICatalogue catalogue;

        public CatalogueController(ICatalogue _catalogue)
        {
            catalogue = _catalogue ?? throw new ArgumentNullException(nameof(_catalogue));
        }

        // POST: api/Search/Clients
        [HttpPost]
        public async Task<ActionResult<List<Client>>> GetClientsByQuery([FromBody] searchQueryModel model)
        {
            try
            {
                List<Logic.Objects.Client> listLogicClient = await catalogue.searchClientByQuery(model);

                return Ok(listLogicClient);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
