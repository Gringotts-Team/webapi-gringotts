using gringotts_application.DTOs;
using gringotts_application.Exceptions;
using gringotts_application.Models;
using gringotts_application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gringotts_api.Controllers
{
    /// <summary>
    /// Controller for managing mage-related
    /// </summary>
    [ApiController]
    [Route("mages")]
    public class MagesController : Controller
    {
        private readonly IMageService _mageService;

        /// <summary>
        /// Constructor for MageController.
        /// </summary>
        /// <param name="mageService">Service for handling mage</param>
        public MagesController(IMageService mageService)
        {
            _mageService = mageService;
        }

        /// <summary>
        /// Retrieves a list of mages based on the provided filtering criteria.
        /// </summary>
        /// <param name="mageName">The name of the mage to filter by.</param>
        /// <param name="AALN">The Arcane Authorization Licence Number (AALN) of the mage to filter by.</param>
        /// <param name="minAge">The minimum age of the mage to filter by.</param>
        /// <param name="maxAge">The maximum age of the mage to filter by.</param>
        /// <param name="houseId">The ID of the house to which the mage belongs.</param>
        /// <param name="minRegDate">The minimum registration date of the mage to filter by.</param>
        /// <param name="maxRegDate">The maximum registration date of the mage to filter by.</param>
        /// <returns>
        /// An ActionResult containing a list of MageListModel objects representing the filtered mages.
        /// </returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Overseer, Minion")]
        public async Task<ActionResult<List<MageListModel>>> GetMageList(
            [FromQuery] string? mageName,
            [FromQuery] string? AALN,
            [FromQuery] int? minAge,
            [FromQuery] int? maxAge,
            [FromQuery] int? houseId,
            [FromQuery] DateTime? minRegDate,
            [FromQuery] DateTime? maxRegDate)
        {
            try
            {
                List<MageListModel> mages = await _mageService.GetMageList(mageName, AALN, minAge, maxAge, houseId, minRegDate, maxRegDate);
                return Ok(mages);
            }
            catch (ApiException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        /// <summary>
        /// Create a new mage.
        /// </summary>
        /// <param name="mageModel">Mage to be created.</param>
        /// <returns>mage created if it was successful, otherwise it is an exception.</returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Overseer")]
        public async Task<ActionResult<MageDTO>> CreateMage(MageModel mageModel)
        {
            try
            {
                MageDTO mageCreated = await _mageService.CreateMage(mageModel);
                return Ok(mageCreated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a mage by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the mage.</param>
        /// <returns>Returns the mage if found, otherwise returns a BadRequest.</returns>
        [HttpGet]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Overseer")]

        public async Task<ActionResult<MageDTO>> GetMage(int id)
        {
            try
            {
                MageDTO mage = await _mageService.GetMage(id);
                return Ok(mage);
            }
            catch (ApiException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing mage by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the mage to be updated.</param>
        /// <param name="mageModel">The mage model containing updated information.</param>
        /// <returns>Returns the updated mage if successful, otherwise returns a BadRequest.</returns>
        [HttpPut]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Overseer")]

        public async Task<ActionResult<MageDTO>> UpdateMage(int id, MageModel mageModel)
        {
            try
            {
                MageDTO mage = await _mageService.UpdateMage(id, mageModel);
                return Ok(mage);
            }
            catch (ApiException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
