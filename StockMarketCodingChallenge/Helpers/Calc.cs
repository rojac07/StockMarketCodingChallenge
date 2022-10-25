using DomainModels;
using StockMarketCodingChallengeWpfApp.Interfaces;
using System.Collections.Generic;

namespace StockMarketCodingChallengeWpfApp.Helpers
{
    public static class Calc
    {
        public static double Max(List<Candle> candles)
        {
            double max = double.MinValue;
            foreach (var candle in candles)
            {
                if (candle.Close > max)
                    max = candle.Close;
            }
            return max;
        }

        public static double Min(List<Candle> candles)
        {
            double min = double.MaxValue;
            foreach (var candle in candles)
            {
                if (candle.Close < min)
                    min = candle.Close;
            }
            return min;
        }

        public static double GetScaleFactor(double a, double b) => a / b;

        public static double TotalAssetValue(Wallet wallet, double currentStockPrice)
        {
            var totalValue = wallet.Balance + wallet.Stocks * currentStockPrice;
            return totalValue;
        }
    }
}
