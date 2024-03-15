namespace gringotts_application.Models
{
    /// <summary>
    /// Model representating a mage in the Gringotts application.
    /// </summary>
    public class MageModel
    {
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

    }
}
