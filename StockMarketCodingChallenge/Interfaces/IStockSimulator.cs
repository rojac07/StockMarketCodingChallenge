using DomainModels;
using System;

namespace StockMarketCodingChallengeWpfApp
{
    public interface IStockSimulator
    {
        ///// <summary>
        ///// Event is triggered on new trade day. 
        ///// </summary>
        //event EventHandler OnNewTradeDayEvent;

        /// <summary>
        /// Initialized all required data to start a single game/challenge.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="symbol"></param>
        /// <returns>return true when game can be started.</returns>
        bool NewGame(DateRange dateRange, string symbol = null);

        /// <summary>
        /// Initialized all required data to start number of challenges.
        /// </summary>
        /// <param name="dateRange"></param>
        /// <param name="randomStockListCount"></param>
        /// <returns></returns>
        bool NewGame(DateRange dateRange, int randomStockListCount);

        /// <summary>
        /// Start game or continue if it is paused.
        /// </summary>
        void Start();

        /// <summary>
        /// Pause game.
        /// </summary>
        void Pause();

        /// <summary>
        /// Stop game.
        /// </summary>
        void Stop();

        /// <summary>
        /// Save resuls of the game.
        /// </summary>
        void SaveResults();
    }
}