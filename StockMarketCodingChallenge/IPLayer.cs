using DomainModels;
using StockMarketCodingChallengeWpfApp.Players;
using System;

namespace StockMarketCodingChallengeWpfApp
{
    public class Action : IAction
    {
        private readonly IPLayer player;
        private readonly double currentStockPrice;
        public Action(IPLayer player, double stockPrice)
        {
            this.player = player;
            this.currentStockPrice = stockPrice;
        }
        public void Buy(double amount)
        {
            if (currentStockPrice == 0)
                throw new Exception("Stock price cannot be zero(0)");

            double amountToSpend = amount * currentStockPrice;

            if (player.MyWallet.Balance >= amountToSpend)
            {
                player.MyWallet.Balance -= amountToSpend;
                player.MyWallet.Stocks += amount;
                Console.WriteLine("{0} bought: {1} stocks. Spend: {2}$", player.Name, amount, amountToSpend);
            }
        }

        public void Sell(double amount)
        {
            if(player.MyWallet.Stocks >= amount)
            {
                if (currentStockPrice == 0)
                    throw new Exception("Stock price cannot be zero(0)");

                double amountToReceive = amount * currentStockPrice;
                player.MyWallet.Stocks -= amount;
                player.MyWallet.Balance += amountToReceive;
                Console.WriteLine("{0} sold: {1} stocks. Received: {2}$", player.Name, amount, amountToReceive);
            }
        }

        public void Wait()
        {
            //Do nothing..
        }
    }

    public interface IAction
    {        
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

    public interface IPLayer
    {
        string Name { get; }

        Wallet MyWallet { get; }

        void OnAction(double stockCurrentPrice, StockHistory stockHistory, IAction action);
    }
}
