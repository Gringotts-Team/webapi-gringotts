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
        /// Retrieves a list of mages based on specified filtering criteria.
        /// </summary>
        /// <param name="mageName">The name of the mage to filter by.</param>
        /// <param name="AALN">The AALN of the mage to filter by.</param>
        /// <param name="minAge">The minimum age of the mage to filter by.</param>
        /// <param name="maxAge">The maximum age of the mage to filter by.</param>
        /// <param name="houseId">The ID of the house to which the mage belongs.</param>
        /// <param name="minRegDate">The minimum registration date of the mage to filter by.</param>
        /// <param name="maxRegDate">The maximum registration date of the mage to filter by.</param>
        /// <returns>A task representing the asynchronous operation that returns a list of MageListModel objects.</returns>
        Task<List<MageListModel>> GetMageList(string? mageName, string? AALN, int? minAge, int? maxAge, int? houseId, DateTime? minRegDate, DateTime? maxRegDate);

        /// <summary>
        /// Create a new mage.
        /// </summary>
        /// <param name="mageModel">mage to be created</param>
        /// <returns>Mage created if successful, null otherwise</returns>
        ///
        Task<MageDTO> CreateMage(MageModel mageModel);

        /// <summary>
        /// Retrieves a mage by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the mage.</param>
        /// <returns>Returns the mage if found, otherwise returns a BadRequest.</returns>
        Task<MageDTO> GetMage(int id);

        /// <summary>
        /// Updates an existing mage by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the mage to be updated.</param>
        /// <param name="mageModel">The mage model containing updated information.</param>
        /// <returns>Returns the updated mage if successful, otherwise returns a BadRequest.</returns>
        Task<MageDTO> UpdateMage(int id, MageModel mageModel);
    }
}
