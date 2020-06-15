using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Santa.Logic.Interfaces;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IRepository repository;
        public MessageController(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));

        }
        // GET: api/Message
        /// <summary>
        /// Gets all messages sent in DB
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<Logic.Objects.Message>> Get()
        {
            try
            {
                return Ok(repository.GetAllMessages());
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        // GET: api/Message/5
        /// <summary>
        /// Gets certain message by ID
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns></returns>
        [HttpGet("{messageID}", Name = "Get")]
        public async Task<ActionResult<Logic.Objects.Message>> Get(Guid messageID)
        {
            try
            {
                return Ok(await repository.GetMessageByIDAsync(messageID));
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
            
        }

        // POST: api/Message
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
    }
}
