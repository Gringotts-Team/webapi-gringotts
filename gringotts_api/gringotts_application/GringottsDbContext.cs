using gringotts_application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace gringotts_application
{
    /// <summary>
    /// Database context for Gringotts application.
    /// </summary>
    public class GringottsDbContext : DbContext
    {
        /// <summary>
        /// Constructor for GringottsDbContext.
        /// </summary>
        /// <param name="options">The options to be used by the context.</param>
        public GringottsDbContext(DbContextOptions<GringottsDbContext> options) : base(options) 
        {
        }

        /// <summary>
        /// Gets or sets the DbSet for User entities in the database.
        /// </summary>
        public DbSet<UserLoginDTO> users { get; set; }
        
        /// <summary>
        /// Gets or sets the DbSet for Houses entities in the database.
        /// </summary>
        public DbSet<HouseDTO> houses { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for Mages entities in the database.
        /// </summary>
        public DbSet<MageDTO> mages { get; set; }
    }
}
