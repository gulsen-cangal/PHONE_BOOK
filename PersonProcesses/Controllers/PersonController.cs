using Microsoft.AspNetCore.Mvc;
using PersonProcesses.API.Data;
using PersonProcesses.API.Services.Interfaces;

namespace PersonProcesses.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {

        private readonly IPersonService personService;
        private readonly IContactInformationService contactInformationService;

        public PersonController(IPersonService personService, IContactInformationService contactInformationService)
        {
            this.personService = personService;
            this.contactInformationService = contactInformationService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReturnData>> AddPerson([FromBody] PersonData personData)
        {
            if (personData == null)
                return BadRequest();

            var result = await personService.AddPerson(personData);

            if (result.Response == true)
                return Created("", result);

            return BadRequest(result.Message);
        }

        [HttpDelete("{personId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReturnData>> DeletePerson([FromRoute] Guid personId)
        {
            var result = await personService.DeletePerson(personId);

            if (result.Response == false)
            {
                return NotFound(result.Message);
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReturnData>> GetAllPersons()
        {
            var allPersons = await personService.GetAllPersons();

            if (allPersons.Response == false)
            {
                return NotFound(allPersons.Message);
            }
            else
            {
                return Ok(allPersons);
            }
        }

        [HttpGet("{personId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReturnData>> GetPerson(Guid personId)
        {
            var person = await personService.GetPerson(personId);

            if (person.Response == false)
            {
                return NotFound(person.Message);
            }
            else
            {
                return Ok(person);
            }
        }

        [HttpPost("{personId}/ContactInformations")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReturnData>> AddContactInformation(Guid personId, [FromBody] ContactInformationData contactInformationData)
        {
            if (contactInformationData == null)
            {
                return BadRequest();
            }

            var result = await contactInformationService.AddContactInformation(personId, contactInformationData);

            if (result.Response == true)
            {
                return Created("", result);
            }
            else
            {
                return NotFound(result.Message);
            }

        }

        [HttpDelete("ContactInformations/{contactInformationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReturnData>> DeleteContactInformation([FromRoute] Guid contactInformationId)
        {

            var result = await contactInformationService.DeleteContactInformation(contactInformationId);

            if (result.Response == false)
            {
                return NotFound(result.Message);
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("{personId}/Detail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReturnData>> GetPersonDetail(Guid personId)
        {
            var result = await personService.GetPersonDetail(personId);

            if (result.Response == false)
            {
                return NotFound(result.Message);
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("ContactInformations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReturnData>> GetAllContactInformations()
        {
            var result = await contactInformationService.GetAllContactInformations();

            if (result.Response == false)
            {
                return NotFound(result.Message);
            }
            else
            {
                return Ok(result);
            }
        }
    }
}
