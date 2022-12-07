using DomainModels;
using System;
using System.Collections.Generic;

namespace StockMarketCodingChallengeWpfApp.Repositories
{
    /// <summary>
    /// Store and get stock data from the file.
    /// </summary>
    public interface IStockDataFileRepository
    {
        /// <summary>
        /// Get all available stock data for a given symbol.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        List<Candle> Get(string symbol);

        /// <summary>
        /// Save candles in
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="candles"></param>
        void Save(string symbol, List<Candle> candles);
    }
}
