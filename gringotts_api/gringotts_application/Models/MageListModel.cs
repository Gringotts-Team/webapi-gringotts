
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gringotts_application.Models
{
    /// <summary>
    /// Model representating a mage in the Gringotts application.
    /// </summary>
    public class MageListModel
    {
        [Key]
        public int mag_id { get; set; }

        /// <summary>
        /// Gets or sets the aaln of the mage.
        /// </summary>
        public string mag_aaln { get; set; }

        /// <summary>
        /// Gets or sets the fullname of the mage.
        /// </summary>
        public string mag_name { get; set; }

        /// <summary>
        /// Gets or sets the house of the mage.
        /// </summary>
        public HouseModel mag_house { get; set; }


        /// <summary>
        /// Gets or sets the age of the mage.
        /// </summary>
        public int mag_age { get; set; }

        /// <summary>
        /// Gets or sets the inscriptiondate of the mage.
        /// </summary>
        public DateTime mag_inscription { get; set; }


    }
}
