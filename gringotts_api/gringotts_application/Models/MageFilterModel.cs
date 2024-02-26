using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gringotts_application.Models
{
    /// <summary>
    /// Represents the request model for filtering mage list.
    /// </summary>
    public class MageFilterModel
    {
        /// <summary>
        /// Gets or sets the name of the mage.
        /// </summary>
        public string? mageName { get; set; }

        /// <summary>
        /// Gets or sets the AALN of the mage.
        /// </summary>
        public string? AALN { get; set; }

        /// <summary>
        /// Gets or sets the minimum age of the mage.
        /// </summary>
        public int? minAge { get; set; }

        /// <summary>
        /// Gets or sets the maximum age of the mage.
        /// </summary>
        public int? maxAge { get; set; }

        /// <summary>
        /// Gets or sets the house id of the mage house.
        /// </summary>
        public int? houseId { get; set; }

        /// <summary>
        /// Gets or sets the minimum registration date of the mage.
        /// </summary>
        public DateTime? minRegDate { get; set; }

        /// <summary>
        /// Gets or sets the maximum registration date of the mage.
        /// </summary>
        public DateTime? maxRegDate { get; set; }
    }

}
