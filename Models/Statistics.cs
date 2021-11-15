using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework1.Models
{
    /// <summary>
    /// Represents a data with a name, a value and a percentage according to a certain total
    /// </summary>
    public class Statistics
    {
        public string Text { get; set; }
        public double Count { get; set; }
        public double Ratio { get; set; }
        public string Percentage { get; set; }
        /// <summary>
        /// Represents a data with a name, a value and a percentage according to a certain total
        /// </summary>
        /// <param name="text">Name of the date</param>
        /// <param name="count">Value of the data</param>
        /// <param name="totalFromStat">Total value from dataset the data comes from</param>
        public Statistics(string text, double count, double totalFromStat)
        {
            Text = text;
            Count = count;
            Ratio = Count / totalFromStat * 100;
            Ratio = Math.Round(Ratio, 2);
            Percentage = $"{Ratio}%";
        }
    }
}
