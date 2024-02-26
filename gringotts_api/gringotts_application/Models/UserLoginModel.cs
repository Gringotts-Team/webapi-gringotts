namespace gringotts_application.Models
{
    /// <summary>
    /// Model representating a user in the Gringotts application.
    /// </summary>
    public class UserLoginModel
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public string password { get; set; }
    }
}
