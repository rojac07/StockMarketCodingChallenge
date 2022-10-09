using System;

namespace StockMarketCodingChallengeWpfApp
{
    public interface IStockSimulator
    {
        /// <summary>
        /// Event is triggered on new trade day. 
        /// </summary>
        event EventHandler OnNewTradeDayEvent;

        /// <summary>
        /// Initialized all required data to start a game/challenge.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="symbol"></param>
        /// <returns>return true when game can be sytarted.</returns>
        bool CreateNewChallenge(DateTime startDate, DateTime endDate, string symbol = null);

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
    }
}