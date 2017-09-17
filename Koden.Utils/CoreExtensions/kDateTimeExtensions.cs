#region License
// Copyright (c) 2014 Tim Fischer
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the 'Software'), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
using System;
using Koden.Utils.Models;


namespace Koden.Utils.Extensions
{
    /// <summary>
    /// Common Extension Methods to be used in Console and/or Web applications adhering to Koden Standards
    /// </summary>
    public static partial class ExtensionMethods
    {

        /// <summary>
        /// Returns all information relative to a Business Quarter
        /// </summary>
        /// <param name="dateValue">The date.</param>
        /// <param name="maxToday">if set to <c>true</c> LastDayOfPeriod will max out at todays date if it is greater than today.</param>
        /// <returns><see cref="QuarterInfo"/></returns>
        public static QuarterInfo kQuarterInfo(this DateTime dateValue, bool maxToday = true)
        {

            var retVal = new QuarterInfo
            {
                Qtr = (int)Math.Ceiling(dateValue.Month / 3.0)
            };
            retVal.FirstDayOfPeriod = new DateTime(dateValue.Year, ((retVal.Qtr - 1) * 3) + 1, 1);
            retVal.LastDayOfPeriod = retVal.FirstDayOfPeriod.AddMonths(3).AddDays(-1);

            if (retVal.LastDayOfPeriod > DateTime.Now && maxToday) retVal.LastDayOfPeriod = DateTime.Now;

            return retVal;
        }

        /// <summary>
        /// Returns all information relative to Fiscal Quarters
        /// </summary>
        /// <param name="dateValue">The date.</param>
        /// <param name="fiscalMonthStart">The fiscal month start date.</param>
        /// <param name="maxToday">if set to <c>true</c> LastDayOfFiscalPeriod will max out at todays date if it is greater than today.</param>
        /// <returns><see cref="FiscalInfo"/></returns>
        public static FiscalInfo kFiscalInfo(this DateTime dateValue, int fiscalMonthStart, bool maxToday = true)
        {

            var retVal = new FiscalInfo();

            var fMonth = (int)fiscalMonthStart;
            //Excel Formula
            //= MOD(CEILING(22 + MONTH(A2) - 9 - 1, 3) / 3, 4) + 1
            retVal.FiscalQtr = (int)(((Math.Ceiling((decimal)22 + dateValue.Month - fMonth - 1) / 3) + 1) % 4) + 1;


            //retVal.FirstDayOfPeriod = new DateTime(dateValue.Year, ((retVal.Qtr - 1) * 3) + 1, 1);
            var newYear = dateValue.Year;
            var startMonth = ((retVal.FiscalQtr - 1) * 3) + fMonth;
            if (startMonth < fMonth) newYear++;
            if (startMonth > 12) startMonth -= 12;

            retVal.FirstDayOfFiscalPeriod = new DateTime(newYear, startMonth, 1);
            retVal.LastDayOfFiscalPeriod = retVal.FirstDayOfFiscalPeriod.AddMonths(3).AddDays(-1);
            if (retVal.LastDayOfFiscalPeriod > DateTime.Now && maxToday) retVal.LastDayOfFiscalPeriod = DateTime.Now;

            return retVal;
        }
        /// <summary>
        /// Returns all information relative to a Business Quarter (or Fiscal year quarters)
        /// SemiAnnual Dates are 10/1 - 3/31 and 4/1 - 9/30 (Default)
        /// !Default is HARDCODED, fiscal year change will require method modification!
        /// </summary>
        /// <param name="dateValue">The date.</param>
        /// <param name="maxToday">if set to <c>true</c> LastDayOfQuarter will max out at todays date if it is greater than today.</param>
        /// <returns><see cref="QuarterInfo"/></returns>
        public static QuarterInfo kSemiAnnualInfo(this DateTime dateValue, bool maxToday = true)
        {

            var retVal = new QuarterInfo
            {
                Qtr = (dateValue.Month + 2) / 3
            };

            switch (retVal.Qtr)
            {
                case 1:
                    retVal.FirstDayOfPeriod = new DateTime(dateValue.Year - 1, 10, 1);
                    retVal.LastDayOfPeriod = new DateTime(dateValue.Year, 3, 31);
                    break;
                case 2:
                case 3:
                    retVal.FirstDayOfPeriod = new DateTime(dateValue.Year, 4, 1);
                    retVal.LastDayOfPeriod = new DateTime(dateValue.Year, 9, 30);
                    break;
                case 4:
                    retVal.FirstDayOfPeriod = new DateTime(dateValue.Year, 10, 1);
                    retVal.LastDayOfPeriod = new DateTime(dateValue.Year + 1, 3, 31);
                    break;
            }

            if (retVal.LastDayOfPeriod > DateTime.Now && maxToday) retVal.LastDayOfPeriod = DateTime.Now;

            if (retVal.Qtr == 3) retVal.Qtr = 2;
            if (retVal.Qtr == 4) retVal.Qtr = 1;

            return retVal;
        }

        /// <summary>
        /// Returns true if the date is between or equal to one of the two values.
        /// </summary>
        /// <param name="date">DateTime Base, from where the calculation will be preformed.</param>
        /// <param name="startDateValue">The start date.</param>
        /// <param name="endDateValue">The end date.</param>
        /// <returns>
        /// boolean value indicating if the date is between or equal to one of the two values
        /// </returns>
        public static bool kBetween(this DateTime date, DateTime startDateValue, DateTime endDateValue)
        {
            return date.Ticks >= startDateValue.Ticks && date.Ticks <= endDateValue.Ticks;
        }

        /// <summary>
        /// Calculates the age of something/someone.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>Integer representing the age in years</returns>
        public static int kCalculateAge(this DateTime dateTime)
        {
            var age = DateTime.Now.Year - dateTime.Year;
            if (DateTime.Now < dateTime.AddYears(age))
                age--;
            return age;
        }
        /// <summary>
        /// Counts the days between.
        /// </summary>
        /// <param name="startDateValue">The start date.</param>
        /// <param name="endDateValue">The end date.</param>
        /// <returns>Integer representing the days between two given dates</returns>
        public static int kCountDaysBetween(this DateTime startDateValue, DateTime endDateValue)
        {
            TimeSpan span = endDateValue.Subtract(startDateValue);
            return (int)span.TotalDays;
            //return  (endDate - startDate).Days;
        }

        /// <summary>
        /// Displays a readable time
        /// </summary>
        /// <param name="value">The DateTime.</param>
        /// <returns>string in the form of " one second ago, a minute ago, etc"</returns>
        public static string kAsReadableTime(this DateTime value)
        {
            var ts = new TimeSpan(DateTime.Now.Ticks - value.Ticks);
            double delta = ts.TotalSeconds;
            if (delta < 60)
            {
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
            }
            if (delta < 120)
            {
                return "a minute ago";
            }
            if (delta < 2700) // 45 * 60
            {
                return ts.Minutes + " minutes ago";
            }
            if (delta < 5400) // 90 * 60
            {
                return "an hour ago";
            }
            if (delta < 86400) // 24 * 60 * 60
            {
                return ts.Hours + " hours ago";
            }
            if (delta < 172800) // 48 * 60 * 60
            {
                return "yesterday";
            }
            if (delta < 2592000) // 30 * 24 * 60 * 60
            {
                return ts.Days + " days ago";
            }
            if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            var years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "one year ago" : years + " years ago";
        }
        /// <summary>
        /// Get the date for the beginning of the week.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>Date for the beginning of the week</returns>
        public static DateTime kBeginningOfWeek(this DateTime dateTime)
        {
            int dayOfWeek = (int)dateTime.DayOfWeek;
            dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;
            DateTime beginningOfWeek = dateTime.AddDays(1 - dayOfWeek);

            return beginningOfWeek;
        }
        /// <summary>
        /// Returns 12:59:59pm time for the date passed.
        /// Useful for date only search ranges end value
        /// </summary>
        /// <param name="dateValue">Date to convert</param>
        /// <returns><see cref="DateTime"/></returns>
        public static DateTime kEndOfDay(this DateTime dateValue)
        {
            return dateValue.Date.AddDays(1).AddMilliseconds(-1);
        }

        /// <summary>
        /// Returns 12:00am time for the date passed.
        /// Useful for date only search ranges start value
        /// </summary>
        /// <param name="dateValue">Date to convert</param>
        /// <returns><see cref="DateTime"/></returns>
        public static DateTime kBeginningOfDay(this DateTime dateValue)
        {
            return dateValue.Date;
        }

        /// <summary>
        /// Returns the very end of the given month (the last millisecond of the last hour for the given date)
        /// </summary>
        /// <param name="dateValue">DateTime Base, from where the calculation will be preformed.</param>
        /// <returns>Returns the very end of the given month (the last millisecond of the last hour for the given date) - <see cref="DateTime"/> </returns>
        public static DateTime kEndOfMonth(this DateTime dateValue)
        {
            return new DateTime(dateValue.Year, dateValue.Month, DateTime.DaysInMonth(dateValue.Year, dateValue.Month), 23, 59, 59, 999);
        }

        /// <summary>
        /// Returns the Start of the given month (the fist millisecond of the given date)
        /// </summary>
        /// <param name="dateValue">DateTime Base, from where the calculation will be preformed.</param>
        /// <returns>Returns the Start of the given month (the fist millisecond of the given date) - <see cref="DateTime"/></returns>
        public static DateTime kBeginningOfMonth(this DateTime dateValue)
        {
            return new DateTime(dateValue.Year, dateValue.Month, 1, 0, 0, 0, 0);
        }

    }
}
