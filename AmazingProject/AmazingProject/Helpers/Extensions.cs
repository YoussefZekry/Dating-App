using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmazingProject.Helpers
{
    public static class Extensions
    {
        public static int CalculateAge(this DateTime thedateTime)
        {
            var age = DateTime.Today.Year - thedateTime.Year;
            if (thedateTime.AddYears(age)>DateTime.Today)
                age--;

                return age;
        }
    }
}
