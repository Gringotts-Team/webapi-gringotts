using System.ComponentModel.DataAnnotations;

namespace gringotts_application.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for user login information.
    /// </summary>
    public class UserLoginDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [Key]
        public int iduser { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the role of the user.
        /// </summary>
        public string role { get; set; }

        /// <summary>
        /// Gets or set the file path or URL to the user's profile picture.
        /// </summary>
        public string profile_picture { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public string password { get; set; }

    }
}
