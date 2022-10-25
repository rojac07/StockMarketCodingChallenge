using System;

namespace DomainModels
{
    public class Candle
    {
        /// <summary>
        /// Symbol 
        /// </summary>
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        
        /// <summary>
        /// Lowest trading price for the current day.
        /// </summary>
        public double Low { get; set; }

        /// <summary>
        /// Stock price upon trading day closed. 
        /// </summary>
        public double Close { get; set; }
    }
}
