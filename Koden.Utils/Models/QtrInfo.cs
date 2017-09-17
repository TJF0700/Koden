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

namespace Koden.Utils.Models
{
    /// <summary>
    /// Get Quarter information about DateTime
    /// </summary>
    public class QuarterInfo
    {
        /// <summary>
        /// Gets or sets the Quarter.
        /// </summary>
        /// <value>
        /// The QTR.
        /// </value>
        public int Qtr { get; set; }
        /// <summary>
        /// Gets or sets the first day of quarter.
        /// </summary>
        /// <value>
        /// The first day of quarter.
        /// </value>
        public DateTime FirstDayOfPeriod { get; set; }
        /// <summary>
        /// Gets or sets the last day of quarter.
        /// </summary>
        /// <value>
        /// The last day of quarter.
        /// </value>
        public DateTime LastDayOfPeriod { get; set; }

        /// <summary>
        /// Returns Quarter
        /// </summary>
        /// <returns>"1st Quarter", "2nd Quarter", "3rd Quarter", "4th Quarter"</returns>
        public string ToLongString(int qtr)
        {
            string[] qString = { "1st Quarter", "2nd Quarter", "3rd Quarter", "4th Quarter" };
            return qString[qtr - 1];
        }
        /// <summary>
        /// Returns Q1,Q2,Q3,Q4
        /// </summary>
        /// <returns> "Q1", "Q2", "Q3", "Q4"</returns>
        public string ToShortString(int qtr)
        {
            string[] qString = { "Q1", "Q2", "Q3", "Q4" };
            return qString[qtr - 1];
        }

        /// <summary>
        /// Returns "1st Half", "2nd Half"
        /// </summary>
        /// <returns>"1st Half", "2nd Half"</returns>
        public string ToSemiAnnualString(int qtr)
        {
            string[] qString = { "1st Half", "2nd Half" };
            if (qtr > 2) qtr = 1;
            else qtr = 0;
            return qString[qtr];
        }
    }


    /// <summary>
    /// Get Fiscal Information extension
    /// </summary>
    public class FiscalInfo
    {

        /// <summary>
        /// Gets or sets the fiscal Quarter.
        /// </summary>
        /// <value>
        /// The fiscal Quarter.
        /// </value>
        public int FiscalQtr { get; set; }
        /// <summary>
        /// Gets or sets the first day of fiscal period.
        /// </summary>
        /// <value>
        /// The first day of fiscal period.
        /// </value>
        public DateTime FirstDayOfFiscalPeriod { get; set; }
        /// <summary>
        /// Gets or sets the last day of fiscal period.
        /// </summary>
        /// <value>
        /// The last day of fiscal period.
        /// </value>
        public DateTime LastDayOfFiscalPeriod { get; set; }
        /// <summary>
        /// Returns Quarter
        /// </summary>
        /// <returns>"1st Quarter", "2nd Quarter", "3rd Quarter", "4th Quarter"</returns>
        public string ToLongString(int qtr)
        {
            string[] qString = { "1st Quarter", "2nd Quarter", "3rd Quarter", "4th Quarter" };
            return qString[qtr - 1];
        }
        /// <summary>
        /// Returns Q1,Q2,Q3,Q4
        /// </summary>
        /// <returns> "Q1", "Q2", "Q3", "Q4"</returns>
        public string ToShortString(int qtr)
        {
            string[] qString = { "Q1", "Q2", "Q3", "Q4" };
            return qString[qtr - 1];
        }

        /// <summary>
        /// Returns "1st Half", "2nd Half"
        /// </summary>
        /// <returns>"1st Half", "2nd Half"</returns>
        public string ToSemiAnnualString(int qtr)
        {
            string[] qString = { "1st Half", "2nd Half" };
            if (qtr > 2) qtr = 1;
            else qtr = 0;
            return qString[qtr];
        }
    }
}

