using AareonTechnicalTest.Models;
using AareonTechnicalTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace AareonTechnicalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private ITicketService _ticketService {get; set;}
        private ILogger _logger; 

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="ticketService"></param>
        public TicketsController(ITicketService ticketService, ILogger<TicketsController> logger)
        {
            this._ticketService = ticketService;
            this._logger = logger;
        }

        /// <summary>
        /// Get all tickets
        /// </summary>
        /// <remarks>
        /// I would usually implement paging with default values
        /// </remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public ActionResult<IEnumerable<Ticket>> Get()
        {
            return Ok(this._ticketService.GetAllTickets());
        }

        /// <summary>
        /// Get one ticket by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<Ticket> Get(int id)
        {
            this._logger.LogInformation("GetTicketById");

            if (this._ticketService.TicketExists(id))
            {
                return Ok(this._ticketService.GetTicketById(id));
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create a new ticket
        /// </summary>
        /// <param name="ticket"></param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public ActionResult Post([FromBody] Ticket ticket)
        {
            // this is a simple example of how activity can be audited and monitored using a configurable and DI logger
            // the user's IP address, username, browser type, etc can be obtained and logged. The logger can be configured
            // to log to a database, cloud logging system, and generate emails for monitored events.
            this._logger.LogInformation("CreateNewTicket");

            Ticket created = this._ticketService.CreateTicket(ticket);

            if(created.Id > 0)
            {
                return Created($"/api/tickets/{created.Id}", created);
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Update an existing ticket
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ticket"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Ticket ticket)
        {
            if (this._ticketService.TicketExists(id))
            {
                if (this._ticketService.UpdateTicket(id, ticket))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Delete an existing ticket
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (this._ticketService.TicketExists(id))
            {
                if (this._ticketService.DeleteTicket(id))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create a note against a ticket
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("/{ticketId}/Persons/{personId}/Note")]
        public ActionResult PostNote(int ticketId, int personId, [FromBody] Note note)
        {
            if (this._ticketService.TicketExists(ticketId))
            {
                Ticket ticket = this._ticketService.CreateTicketNote(ticketId, personId, note);

                if (ticket != null)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Update a note against a ticket
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("/{ticketId}/Persons/{personId}/Note")]
        public ActionResult PutNote(int ticketId, int personId, [FromBody] Note note)
        {
            if (this._ticketService.TicketExists(ticketId))
            {
                Ticket ticket = this._ticketService.UpdateTicketNote(ticketId, personId, note);

                if (ticket != null)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Update a note against a ticket
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("/{ticketId}/Persons/{personId}/Note")]
        public ActionResult DeleteNote(int ticketId, int personId)
        {
            if (this._ticketService.TicketExists(ticketId))
            {

                if (_ticketService.CanPersonDeleteNote(personId))
                {
                    Ticket ticket = this._ticketService.DeleteTicketNote(ticketId, personId);

                    if (ticket != null && string.IsNullOrEmpty(ticket.Note))
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return Unauthorized($"Person [PersonId = {personId}] is not Admin");
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}
