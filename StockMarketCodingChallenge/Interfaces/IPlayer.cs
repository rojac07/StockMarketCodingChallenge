using DomainModels;
using System;

namespace StockMarketCodingChallengeWpfApp.Interfaces
{
    public interface IPlayer
    {
        string Name { get; }

        Wallet MyWallet { get; }

        void OnNewTradeDay(ITradeAction action);
    }
}
