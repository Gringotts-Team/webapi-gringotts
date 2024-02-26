using System.ComponentModel.DataAnnotations;

namespace gringotts_application.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for mages information.
    /// </summary>
    public class MageDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier for the mages.
        /// </summary>
        /// 
        [Key]
        public int mag_id {  get; set; }

        /// <summary>
        /// Gets or sets the fullname of the mage.
        /// </summary>
        public string mag_name { get; set; }

        /// <summary>
        /// Gets or sets the birthdate of the mage.
        /// </summary>
        public DateTime mag_birthdate { get; set; }

        /// <summary>
        /// Gets or sets the houseid of the mage.
        /// </summary>
        public int mag_hou_id { get; set; }

        /// <summary>
        /// Gets or sets the aaln of the mage.
        /// </summary>
        public string mag_aaln { get; set; }

        /// <summary>
        /// Gets or sets the inscriptiondate of the mage.
        /// </summary>
        public DateTime mag_inscription { get; set; }
    }
}
