using gringotts_application.DTOs;
using gringotts_application.Exceptions;
using gringotts_application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gringotts_api.Controllers
{
    /// <summary>
    /// API controller for managing houses.
    /// </summary>
    [ApiController]
    [Route("house")]
    public class HouseController : Controller
    {
        private readonly IHouseService _houseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HouseController"/> class.
        /// </summary>
        /// <param name="houseService">The service for managing houses.</param>
        public HouseController(IHouseService houseService)
        {
            _houseService = houseService;
        }

        /// <summary>
        /// Gets a list of houses.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the list of house DTOs.
        /// </returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Overseer, Minion")]
        public async Task<ActionResult<List<HouseDTO>>> GetHouseList()
        {
            try
            {
                List<HouseDTO> houses = await _houseService.GetHouseList();
                return Ok(houses);
            }
            catch(ApiException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
