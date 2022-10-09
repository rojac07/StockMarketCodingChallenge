using System.Collections.Generic;

namespace DomainModels
{
    public class StockHistory
    {
        /// <summary>
        /// Historical records.
        /// </summary>
        public IList<double> Records { get; set; }
    }
}