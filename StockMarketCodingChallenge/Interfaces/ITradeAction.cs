using DomainModels;

namespace StockMarketCodingChallengeWpfApp.Interfaces
{
    public interface ITradeAction
    {
        /// <summary>
        /// Information about the stock for the current day.
        /// </summary>
        double StockPrice { get; }

        /// <summary>
        /// Amount of stock you will buy.
        /// </summary>
        /// <param name="amount"></param>
        void Buy(double amount);


        /// <summary>
        /// Amount of stocks you will sell.
        /// </summary>
        /// <param name="amount"></param>
        void Sell(double amount);


        /// <summary>
        /// Do nothing.
        /// </summary>
        void Wait();
    }
}
