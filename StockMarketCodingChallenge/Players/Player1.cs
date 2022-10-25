using DomainModels;
using StockMarketCodingChallengeWpfApp.Interfaces;
using System.Collections.Generic;

namespace StockMarketCodingChallengeWpfApp.Players
{
    public class Player1 : IPlayer
    {
        public string Name => "Team 1"; //TODO: add your team name here.. 
      
        public void OnNewTradeDay(ITradeAction action, Wallet myWallet, IList<double> history)
        {
            //TODO impolement your logic here.             
            double stockCurrentPrice = action.StockPrice;
            if (myWallet.Balance > stockCurrentPrice * 25)
            {
                action.Buy(25);
            }
            else if (myWallet.Balance > stockCurrentPrice * 5)
            {
                action.Buy(5);
            }
            else if(myWallet.Balance <= 50)
            {
                var amountToSell = myWallet.Stocks / 4;
                action.Sell(amountToSell);
            }
            else
            {
               action.Wait();
            }
        }

    }
}
