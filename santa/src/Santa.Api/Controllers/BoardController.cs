using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Santa.Api.Models.Board_Entry_Models;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IRepository repository;

        public BoardController(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }

        // GET: api/Board
        /// <summary>
        /// Gets a list of all the board entries posted
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<BoardEntry>>> GetAllEntries()
        {
            try
            {
                List<BoardEntry> logicBoardEntries = await repository.GetAllBoardEntriesAsync();
                return Ok(logicBoardEntries);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }

        }

        // GET: api/Board/5
        /// <summary>
        /// Gets a specific board entry by its board entry ID
        /// </summary>
        /// <param name="boardEntryID"></param>
        /// <returns></returns>
        [HttpGet("{boardEntryID}")]
        public async Task<ActionResult<BoardEntry>> GetBoardEntryByID(Guid boardEntryID)
        {
            try
            {
                BoardEntry logicBoardEntry = await repository.GetBoardEntryByIDAsync(boardEntryID);
                return logicBoardEntry;
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }

        // GET: api/Board/PostNumber/5
        /// <summary>
        /// Gets a board entry by it's post number
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/PostNumber/{postNumber}")]
        public async Task<ActionResult<BoardEntry>> GetBoardEntryByPostNumber(int postNumber)
        {
            try
            {
                BoardEntry logicBoardEntry = await repository.GetBoardEntryByPostNumberAsync(postNumber);
                return logicBoardEntry;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }

        // POST: api/Board
        [HttpPost]
        public async Task<ActionResult<BoardEntry>> PostNewBoardEntry([FromBody] NewBoardEntryModel model)
        {
            try
            {
                BoardEntry newLogicBoardEntry = new BoardEntry()
                {
                    boardEntryID = Guid.NewGuid(),
                    postNumber = model.postNumber,
                    postDescription = model.postDescription
                };
                await repository.UpdateBoardEntryAsync(newLogicBoardEntry);
                await repository.SaveAsync();

                return Ok(await repository.GetBoardEntryByIDAsync(newLogicBoardEntry.boardEntryID));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }

        // PUT: api/Board/5/PostNumber
        /// <summary>
        /// Updates a board entry's post number by its boardEntryID
        /// </summary>
        /// <param name="boardEntryID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{boardEntryID}/PostNumber")]
        public async Task<ActionResult<BoardEntry>> PutPostNumber(Guid boardEntryID, [FromBody] EditPostNumberModel model)
        {
            try
            {
                BoardEntry newLogicBoardEntry = new BoardEntry()
                {
                    boardEntryID = boardEntryID,
                    postNumber = model.postNumber
                };
                await repository.UpdateBoardEntryAsync(newLogicBoardEntry);
                await repository.SaveAsync();
                return Ok(await repository.GetBoardEntryByIDAsync(boardEntryID));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }

        // PUT: api/Board/5/PostDescription
        /// <summary>
        /// Updates a board entry's post description by its boardEntryID
        /// </summary>
        /// <param name="boardEntryID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{boardEntryID}/PostDescription")]
        public async Task<ActionResult<BoardEntry>> PutPostDescription(Guid boardEntryID, [FromBody] EditPostDescriptionModel model)
        {
            try
            {
                BoardEntry newLogicBoardEntry = new BoardEntry()
                {
                    boardEntryID = boardEntryID,
                    postDescription = model.postDescription
                };
                await repository.UpdateBoardEntryAsync(newLogicBoardEntry);
                await repository.SaveAsync();
                return Ok(await repository.GetBoardEntryByIDAsync(boardEntryID));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }

        // DELETE: api/Board/5
        /// <summary>
        /// Deletes a board entry by its ID
        /// </summary>
        /// <param name="boardEntryID"></param>
        /// <returns></returns>
        [HttpDelete("{boardEntryID}")]
        public async Task<ActionResult> DeleteBoardEntry(Guid boardEntryID)
        {
            try
            {
                await repository.DeleteBoardEntryByIDAsync(boardEntryID);
                await repository.SaveAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }
    }
}
