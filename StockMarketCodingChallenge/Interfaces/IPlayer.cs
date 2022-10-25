using DomainModels;
using System.Collections.Generic;

namespace StockMarketCodingChallengeWpfApp.Interfaces
{
    public interface IPlayer
    {
        string Name { get; }

        void OnNewTradeDay(ITradeAction action, Wallet wallet, IList<double> history);
    }
}
