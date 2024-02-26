using System.ComponentModel.DataAnnotations;

namespace gringotts_application.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for houses information.
    /// </summary>
    public class HouseDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier for the houses.
        /// </summary>
        
        [Key]
        public int hou_id {  get; set; }
        /// <summary>
        /// Gets or sets the name of the house.
        /// </summary>
        public string hou_name { get; set; }
    }
}
