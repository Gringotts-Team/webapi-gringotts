using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gringotts_application.Models
{
    /// <summary>
    /// Model representating a houses in the Gringotts application.
    /// </summary>
    public class HouseModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public int hou_id {  get; set; }
        /// <summary>
        /// Gets or sets the name of the house.
        /// </summary>
        public string hou_name { get; set; }
    }
}
