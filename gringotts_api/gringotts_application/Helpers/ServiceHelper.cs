using gringotts_application.DTOs;
using gringotts_application.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace gringotts_application.Helpers
{

    /// <summary>
    /// Helper class for the services to create functions that we will use in various places in our api
    /// </summary>
    public class ServiceHelper
    {

        private readonly GringottsDbContext _context;

        /// <summary>
        /// Constructor for ServiceHelper.
        /// </summary>
        /// <param name="context">Database context for Gringotts.</param>
        public ServiceHelper(GringottsDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Calculates the current age based on the provided birth date.
        /// </summary>
        /// <param name="birthDate">The individual's date of birth.</param>
        /// <returns>The calculated current age in years.</returns>
        public int CurrentAge(DateTime birthDate)
        {
            var age = DateTime.UtcNow.Year - birthDate.Year;

            if (DateTime.UtcNow.Month < birthDate.Month ||
                      (DateTime.UtcNow.Month == birthDate.Month &&
                       DateTime.UtcNow.Day < birthDate.Day))
            {
                age--;
            }
            return age;
        }

        /// <summary>
        /// Validate through a date of birth if the person is under 16 years of age. .
        /// </summary>
        /// <param name="birthDate">date of birth from which validation is to be made</param>
        /// <returns>true if the person is younger than 16 years old, false otherwise</returns>
        /// 
        public bool IsYounger(DateTime birthDate)
        {
            var age = CurrentAge(birthDate);
            
            return age < 16;
        }

        /// <summary>
        /// Validates the AALN (Alphanumeric Age Line Number) of a mage.
        /// </summary>
        /// <param name="mageDTO">The MageDTO object containing AALN information to be validated.</param>
        /// <exception cref="ApiException">Thrown when the AALN validation fails.</exception>
        public async Task AALNValidator(MageDTO mageDTO)
        {
            string pattern = @"^([A-Z]{2})-([A-Z]{2})-(\d{6})$";
            string[] aaln = mageDTO.mag_aaln.Split("-");
            string schoolPart = aaln[0];
            string housePart = aaln[1];
            string numericPart = aaln[2];

            if (!Regex.IsMatch(mageDTO.mag_aaln, pattern))
            {
                var msg = "Invalid AALN format. The format should be: AZ-AZ-123456";
                throw new ApiException(msg);
            }

            var houseInitials = await _context.houses.Where(h => h.hou_id == mageDTO.mag_hou_id)
            .Select(h => h.hou_name.Substring(0, 2))
            .FirstOrDefaultAsync();

            houseInitials = houseInitials?.ToUpper();

            if (houseInitials != null && houseInitials != housePart)
            {
                var msg = "The house indicated in the aaln does not coincide with the house of the mage";
                throw new ApiException(msg);
            }

            if (houseInitials == "OT" && schoolPart != "OT")
            {
                var msg = "For the OT house, the school initials should be OT";
                throw new ApiException(msg);
            }
            else if (houseInitials != "OT" && schoolPart != "HG")
            {
                var msg = "For " + houseInitials + " house, the school initials should be HG";
                throw new ApiException(msg);
            }

            var exists = await _context.mages.AnyAsync(m => m.mag_aaln.Contains(numericPart) && m.mag_id != mageDTO.mag_id);

            if (exists)
            {
                var msg = "AALN already exists";
                throw new ApiException(msg);
            }
        }


    }
}
