using DomainModels;
using System.Collections.Generic;

namespace StockMarketCodingChallengeWpfApp.Helpers
{
    public interface IStockSymbolRepository
    {
        /// <summary>
        /// Get stock symbol by given symbol id. 
        /// For example: 'APPL' is Apple Inc. stock symbol. 
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        StockSymbol Get(string symbol);

        /// <summary>
        /// Get all available stock symbols.
        /// </summary>
        /// <returns></returns>
        IList<StockSymbol> GetAll();
    }
}