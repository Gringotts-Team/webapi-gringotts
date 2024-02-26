using gringotts_application.DTOs;


namespace gringotts_application.Services
{
    /// <summary>
    /// Represents a service for managing houses.
    /// </summary>
    public interface IHouseService
    {
        /// <summary>
        /// Retrieves a list of houses.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of house DTOs.</returns>
        Task<List<HouseDTO>> GetHouseList();
    }
}
