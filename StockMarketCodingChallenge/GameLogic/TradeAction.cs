using DomainModels;
using StockMarketCodingChallengeWpfApp.Helpers;
using StockMarketCodingChallengeWpfApp.Interfaces;
using System;

namespace StockMarketCodingChallengeWpfApp
{
    public class TradeAction : ITradeAction
    {
        private readonly double stockPrice;
        private readonly Wallet wallet;

        public TradeAction(Wallet wallet, double stockPrice)
        {
            this.wallet = wallet;
            this.stockPrice = stockPrice;
        }

        public double StockPrice => stockPrice;

        public void Buy(double amount)
        {
            if (stockPrice == 0)
                throw new Exception("Stock price cannot be zero(0)");

            double amountToSpend = amount * stockPrice;

            if (wallet.Balance >= amountToSpend)
            {
                wallet.Balance -= amountToSpend;
                wallet.Stocks += amount;
                //Console.WriteLine("Bought:  {0} stocks. Spend: {1}$, Total stocks: {2}, Total value: {3}$", amount, amountToSpend, wallet.Stocks, Calc.TotalAssetValue(wallet, stockPrice));
            }
            else
            {
                amount = (int)(wallet.Balance / stockPrice);
                amountToSpend = amount * stockPrice;
                if (amount > 0)
                {
                    wallet.Balance -= amountToSpend;
                    wallet.Stocks += amount;
                    //Console.WriteLine("BOUGHT: {0} stocks. Spend: {1}$, Total stocks: {2}, Total value: {3}$", amount, amountToSpend, wallet.Stocks, Calc.TotalAssetValue(wallet, stockPrice));
                }
            }
        }

        public void Sell(double amount)
        {
            if (wallet.Stocks >= amount)
            {
                if (stockPrice == 0)
                    throw new Exception("Stock price cannot be zero(0)");

                double amountToReceive = amount * stockPrice;
                wallet.Stocks -= amount;
                wallet.Balance += amountToReceive;
                //Console.WriteLine("SOLD: {0} stocks. Spend: {1}$, Total stocks: {2}, Total value: {3}$", amount, amountToReceive, wallet.Stocks, Calc.TotalAssetValue(wallet, stockPrice));
            }
        }

        public void Wait()
        {
            //Do nothing..
        }
    }
}