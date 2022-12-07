using DomainModels;
using StockMarketCodingChallengeWpfApp.Interfaces;
using System;
using System.Collections.Generic;

namespace StockMarketCodingChallengeWpfApp
{
    /// <summary>
    /// Results of the game.
    /// </summary>
    public class GameResults
    {
        /// <summary>
        /// Stock symbol.
        /// </summary>
        public string SymbolName { get; set; }

        /// <summary>
        /// Duration game will take.
        /// </summary>
        public DateRange DateRange { get; set; }

        /// <summary>
        /// Sorted player list. 
        /// </summary>
        public List<Tuple<IPlayer, Wallet>> Players { get; set; }
    }
}
