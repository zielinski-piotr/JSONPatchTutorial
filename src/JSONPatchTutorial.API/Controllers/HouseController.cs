using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JSONPatchTutorial.Contract.Requests;
using JSONPatchTutorial.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JsonPatchTutorial.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HouseController : ControllerBase
    {
        private readonly IHouseService _houseService;
        private readonly ILogger<HouseController> _logger;

        public HouseController(IHouseService houseService, ILogger<HouseController> logger)
        {
            _houseService = houseService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK,
            Type = typeof(IEnumerable<JSONPatchTutorial.Contract.Responses.House.ListItem>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHouses()
        {
            try
            {
                var houses = await _houseService.GetHouses();
                return Ok(houses);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"There was a fatal error while executing {nameof(GetHouses)}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK,
            Type = typeof(JSONPatchTutorial.Contract.Responses.House.Response))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHouseById(Guid id)
        {
            try
            {
                var house = await _houseService.GetHouseById(id);
                return Ok(house);
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogError(e, $"There was an error while executing {nameof(GetHouseById)}");
                return NotFound();
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e, $"There was an error while executing {nameof(GetHouseById)}");
                return BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"There was an error while executing {nameof(GetHouseById)}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchHouse([FromBody] JsonPatchDocument<House.Patch> patchDocument, Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                await _houseService.UpdateHouse(patchDocument, id);
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e, $"There was an error while executing {nameof(PatchHouse)}");
                return BadRequest();
            }
            catch (JsonPatchException e)
            {
                _logger.LogError(e, $"There was an error while executing {nameof(PatchHouse)}");
                return UnprocessableEntity();
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogError(e, $"There was an error while executing {nameof(PatchHouse)}");
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"There was a fatal error while executing {nameof(PatchHouse)}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Accepted();
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHouse(House.Update updateDefinition, Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                await _houseService.UpdateHouse(updateDefinition, id);
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e, $"There was an error while executing {nameof(UpdateHouse)}");
                return BadRequest();
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogError(e, $"There was an error while executing {nameof(UpdateHouse)}");
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"There was a fatal error while executing {nameof(UpdateHouse)}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Accepted();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveHouse(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                await _houseService.RemoveHouseById(id);
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e, $"There was an error while executing {nameof(RemoveHouse)}");
                return BadRequest();
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogError(e, $"There was an error while executing {nameof(RemoveHouse)}");
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"There was a fatal error while executing {nameof(RemoveHouse)}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Accepted();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHouse([FromBody] House.Request request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var house = await _houseService.CreateHouse(request);

                return CreatedAtAction(nameof(GetHouseById), new { id = house.Id }, house);
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e, $"There was an error while executing {nameof(RemoveHouse)}");
                return BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"There was a fatal error while executing {nameof(RemoveHouse)}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}