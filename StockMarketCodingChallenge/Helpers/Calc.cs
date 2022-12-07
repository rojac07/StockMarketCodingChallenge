using DomainModels;
using StockMarketCodingChallengeWpfApp.Interfaces;
using System;
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

        public static double ScaleFactor(double a, double b) => a / b;

        public static double TotalAssetValue(Wallet wallet, double currentStockPrice)
        {
            var totalValue = wallet.Balance + wallet.Stocks * currentStockPrice;
            return totalValue;
        }

        public static string GetSortedPLayerList(IList<Tuple<IPlayer, Wallet>> players, double currentStockPrice)
        {
            string results = string.Empty;
            int standing = 1;
            foreach (var player in players)
            {
                var totalAssets = Math.Round(Calc.TotalAssetValue(player.Item2, currentStockPrice), 0);
                var stocks = Math.Round(player.Item2.Stocks, 0);//MyWallet.Stocks, 0);
                results += $"{standing++}. {player.Item1.Name}\t Stocks: {stocks},\t Total value: {totalAssets} $ \r\n";
            }
            return results;
        }



        //gameResults.Information = $"{dateRange.StartDate.ToShortDateString()} - {dateRange.EndDate.ToShortDateString()}\t{candle.Date.ToShortDateString()}";
    }
}
