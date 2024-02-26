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
    [Route("mage")]
    public class MageController : Controller
    {
        private readonly IMageService _mageService;

        /// <summary>
        /// Constructor for MageController.
        /// </summary>
        /// <param name="mageService">Service for handling mage</param>
        public MageController(IMageService mageService)
        {
            _mageService = mageService;
        }

        /// <summary>
        /// Retrieves a list of mages based on the provided filtering criteria.
        /// </summary>
        /// <param name="filter">The model containing the filtering parameters.</param>
        /// <returns>
        /// An ActionResult containing a list of MageListModel objects representing the filtered mages
        /// </returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Overseer, Minion")]
        [Route("list")]
        public async Task<ActionResult<List<MageListModel>>> GetMageList([FromBody] MageFilterModel filter)
        {
            try
            {
                List<MageListModel> mages = await _mageService.GetMageList(filter);
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
        [Route("/newMage")]
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
    }
}
