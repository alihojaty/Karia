using System;

namespace Karia.Api.Helpers
{
    public static class DateTimeExtensions
    {
        public static int GetCurrentAge(this DateTime dateOfBirth)
        {
            var dateTimeNow = DateTime.Now;
            var age = dateTimeNow.Year - dateOfBirth.Year;
            if (dateTimeNow<dateOfBirth.AddYears(age))
            {
                --age;
            }

            return age;
        }
    }
}