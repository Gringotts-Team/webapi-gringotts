using gringotts_application.DTOs;
using Microsoft.EntityFrameworkCore;


namespace gringotts_application.Services.Imp
{
    public class HouseService : IHouseService
    {

        /// <summary>
        /// Implementation of the <see cref="IHouseService"/> interface for managing houses.
        /// </summary>
        private readonly GringottsDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="HouseService"/> class.
        /// </summary>
        /// <param name="context">The database context for accessing house data.</param>
        public HouseService(GringottsDbContext context)
        {  
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of houses.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the list of house DTOs.
        /// </returns>
        public async Task<List<HouseDTO>> GetHouseList()
        {
            List<HouseDTO> houses = await _context.houses.ToListAsync();
            return houses;
        }
    }
}
