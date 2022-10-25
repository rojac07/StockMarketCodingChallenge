using DomainModels;
using StockMarketCodingChallengeWpfApp.Interfaces;
using System.Collections.Generic;

namespace StockMarketCodingChallengeWpfApp.Players
{
    public class Player2 : IPlayer
    {
        public string Name => "Player 2";  //TODO: add your team name here.. 
      

        public void OnNewTradeDay(ITradeAction action, Wallet wallet, IList<double> history)
        {
            //TODO: implement your logic here...
            //Include history: 30 days..
            action.Buy(1); 
            
        }
    }
}
