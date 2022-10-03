using DomainModels;

namespace StockMarketCodingChallengeWpfApp.Players
{
    public class Player1 : IPLayer
    {
        public string Name => "Team 1"; //TODO: add your team name here.. 
        private readonly Wallet myWallet;
        public Player1(Wallet myWallet)
        {
            this.myWallet = myWallet;
        }

        public void OnAction(double stockCurrentPrice, StockHistory stockHistory, IAction action)
        {
            //TODO impolement your logic here. 

            if(myWallet.Balance > stockCurrentPrice * 25)
            {
                action.Buy(25);
            }
            else if (myWallet.Balance > stockCurrentPrice * 5)
            {
                action.Buy(5);
            }
            else if(myWallet.Balance <= 50)
            {
                var amountToSell = MyWallet.Stocks / 4;
                action.Sell(amountToSell);
            }
            else
            {
                action.Wait();
            }
        }


        public Wallet MyWallet => myWallet;
    }
}
