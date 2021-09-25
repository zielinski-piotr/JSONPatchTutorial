using System;
using System.Threading.Tasks;
using JSONPatchTutorial.Contract.Requests;
using JSONPatchTutorial.Service;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace JsonPatchTutorial.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HouseController : ControllerBase
    {
        private readonly IHouseService _houseService;

        public HouseController(IHouseService houseService)
        {
            _houseService = houseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHouses()
        {
            var houses = await _houseService.GetHouses();
            return Ok(houses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHouseById(Guid id)
        {
            var house = await _houseService.GetHouseById(id);
            return Ok(house);
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> UpdateHouse([FromBody] JsonPatchDocument<House.Patch> patchDocument, Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                await _houseService.UpdateHouse(patchDocument, id);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (InvalidOperationException)
            {
                return Conflict();
            }
            catch (JsonPatchException)
            {
                return UnprocessableEntity();
            }

            return Accepted();
        }
    }
}