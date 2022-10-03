using DomainModels;

namespace StockMarketCodingChallengeWpfApp.Players
{
    public class Player2 : IPLayer
    {
        public string Name => "Player 2";  //TODO: add your team name here.. 
        private readonly Wallet myWallet;
        public Player2(Wallet myWallet)
        {
            this.myWallet = myWallet;
        }

        public Wallet MyWallet => this.myWallet;

        public void OnAction(double stockCurrentPrice, StockHistory stockHistory, IAction action)
        {
            if (IsDownTrend(stockHistory, 3))
                action.Buy(25);
            else if (IsDownTrend(stockHistory, 2))
                action.Buy(15);
            else if (IsUptrend(stockHistory, 5))
                action.Sell(25);
            else if (IsUptrend(stockHistory, 3))
                action.Sell(10);

            action.Wait();
        }


        /// <summary>
        /// Determines how many days stock falls. 
        /// </summary>
        /// <param name="history"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        private bool IsDownTrend(StockHistory history, int threshold)
        {
            double minValue = history.Records[0];
            int count = 0;
            foreach (var value in history.Records)
            {
                if (value < minValue)
                    count++;

                if (count == threshold)
                    return true;
            }
            return false;
        }

        private bool IsUptrend(StockHistory history, int threshold)
        {
            double minValue = history.Records[0];
            int count = 0;
            foreach (var value in history.Records)
            {
                if (value > minValue)
                    count++;

                if (count == threshold)
                    return true;
            }
            return false;
        }
    }
}
