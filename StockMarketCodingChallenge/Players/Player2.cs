using DomainModels;
using StockMarketCodingChallengeWpfApp.Interfaces;

namespace StockMarketCodingChallengeWpfApp.Players
{
    public class Player2 : IPlayer
    {
        public string Name => "Player 2";  //TODO: add your team name here.. 
        private readonly Wallet myWallet;
        public Player2(Wallet myWallet)
        {
            this.myWallet = myWallet;
        }
        public Wallet MyWallet => this.myWallet;

        public void OnNewTradeDay(ITradeAction action)
        {
            //TODO: implement your logic here...
            action.Buy(1);
        }
    }
}
