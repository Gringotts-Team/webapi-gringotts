using gringotts_application.DTOs;
using gringotts_application.Models;


namespace gringotts_application.Services
{
    /// <summary>
    /// Interface for mage service.
    /// </summary>
    public interface IMageService
    {
        /// <summary>
        /// Retrieves a list of MageDTO objects based on specified filtering criteria.
        /// </summary>
        /// <param name="mageName">The name of the mage to filter by. If null, all mages are considered.</param>
        /// <param name="AALN">The AALN (Age at Last Named) of the mage to filter by. If null, all mages are considered.</param>
        /// <param name="minAge">The minimum age of the mage to filter by. If null, no minimum age is applied.</param>
        /// <param name="maxAge">The maximum age of the mage to filter by. If null, no maximum age is applied.</param>
        /// <param name="houseId">The ID of the house to which the mage belongs. If null, all mages are considered.</param>
        /// <param name="minRegDate">The minimum registration date of the mage to filter by. If null, no minimum registration date is applied.</param>
        /// <param name="maxRegDate">The maximum registration date of the mage to filter by. If null, no maximum registration date is applied.</param>
        /// <returns>A task representing the asynchronous operation that returns a list of MageDTO objects.</returns>
        Task<List<MageListModel>> GetMageList(MageFilterModel request);

        /// <summary>
        /// Create a new mage.
        /// </summary>
        /// <param name="mageModel">mage to be created</param>
        /// <returns>Mage created if successful, null otherwise</returns>
        ///
        Task<MageDTO> CreateMage(MageModel mageModel);
    }
}
