using gringotts_application.DTOs;
using gringotts_application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gringotts_application.Helpers
{

    /// <summary>
    /// Helper class for the services to create functions that we will use in various places in our api
    /// </summary>
    public class ServiceHelper
    {
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
    }
}
