using AareonTechnicalTest.Models;
using AareonTechnicalTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AareonTechnicalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private IPersonService _personService {get; set;}

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="ticketService"></param>
        public PersonsController(IPersonService personService)
        {
            this._personService = personService;
        }

        /// <summary>
        /// Get all people
        /// </summary>
        /// <remarks>
        /// I would usually implement paging with default values
        /// </remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public ActionResult<IEnumerable<Person>> Get()
        {
            return Ok(this._personService.GetAllPersons());
        }

        /// <summary>
        /// Get one person by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<Ticket> Get(int id)
        {
            if (this._personService.PersonExists(id))
            {
                return Ok(this._personService.GetPersonById(id));
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create a new person
        /// </summary>
        /// <param name="person"></param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public ActionResult Post([FromBody] Person person)
        {
            Person created = this._personService.CreatePerson(person);

            if(created.Id > 0)
            {
                return Created($"/api/persons/{created.Id}", created);
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Update an existing person
        /// </summary>
        /// <param name="id"></param>
        /// <param name="person"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Person person)
        {
            if (this._personService.PersonExists(id))
            {
                if (this._personService.UpdatePerson(id, person))
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
        /// Delete an existing person
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (this._personService.PersonExists(id))
            {
                if (this._personService.DeletePerson(id))
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
    }
}
