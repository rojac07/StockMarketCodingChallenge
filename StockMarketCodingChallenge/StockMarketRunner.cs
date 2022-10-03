using DomainModels;
using StockMarketCodingChallengeLibrary;
using StockMarketCodingChallengeWpfApp.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using YahooFinanceWebApi;

namespace StockMarketCodingChallengeWpfApp
{
    public class StockMarketRunner
    {
        public void Initialize(DateTime startDate, DateTime endDate, IList<IPLayer> players)
        {
            //Initialize data
            Initialize(startDate, endDate);

            //Create some history..
            int historyCount = 10;
            History = CreateHistoryRecords(historyCount);

            //Challenge is in progress
            //for (int i = 0 + historyCount; i < Candles.Count; i++)
            //{
            //    double stockPriceAtClose = Candles[i].Close;
            //    foreach (var player in players)
            //    {   
            //        player.OnAction(stockPriceAtClose, stockHistory, new Action(player, stockPriceAtClose));
            //        CalculateResults(players, stockPriceAtClose);                    
            //    }
            //    History.Records.Add(stockPriceAtClose);
            //}
        }

        public void OnAction(int i, IList<IPLayer> players)
        {
            if (i + 1 >= Candles.Count)
                return;

            double stockPriceAtClose = Candles[i].Close;
            string date = Candles[i].Date.ToShortDateString();
            foreach (var player in players)
            {
                player.OnAction(stockPriceAtClose, this.History, new Action(player, stockPriceAtClose));
                CalculateResults(players, stockPriceAtClose, date);
            }
            History.Records.Add(stockPriceAtClose);
        }

        public StockHistory History { get; set; }

        private void Initialize(DateTime startDate, DateTime endDate)
        {
            var stockSymbolParser = new StockSymbolParser();
            var stockSymbolList = stockSymbolParser.Parse(@"Resources\nasdaq_screener_1664001254530.csv");
            var yahooWebApiService = new YahooWebApiService();
            var random = new Random();
            StockSymbol randomSymbol;
            string symbol = String.Empty;//"EPAM";
            do
            {
                randomSymbol = GetRandomSymbol(stockSymbolList, random);
                symbol = randomSymbol.Symbol;
                Candles = yahooWebApiService.Download(symbol, startDate, endDate);

            } while (Candles == null);

            CurrentSymbol = symbol;
            Console.WriteLine(randomSymbol.Symbol + "\t" + randomSymbol.Name);
        }

        public void CalculateResults(IList<IPLayer> players, double currentStockPrice, string date)
        {
            WinningTeamResult = string.Empty;
            PlayerListResults = string.Empty;

            var winningPlayer = GetWinningPlayer(players, currentStockPrice);

            WinningTeamResult = "1. " + winningPlayer.Name + " - " +
                "Stocks: " + winningPlayer.MyWallet.Stocks + " - " +
                CalculateTotalAssetValue(winningPlayer, currentStockPrice) + "$";

            //players.Remove(winningPlayer);
            var sortedPlayers = players.OrderByDescending(x => CalculateTotalAssetValue(x, currentStockPrice)).ToList();
            int id = 2;
            for (int i = 1; i < sortedPlayers.Count; i++)
            {
                PlayerListResults += id++.ToString() + ". " + sortedPlayers[i].Name + " - " +
                    "Stocks: " + sortedPlayers[i].MyWallet.Stocks + " - " +
                    CalculateTotalAssetValue(sortedPlayers[i], currentStockPrice) + "$" + "\r\n";
            }
            Info = date;


        }

        private IPLayer GetWinningPlayer(IList<IPLayer> players, double currentStockPrice)
        {
            IPLayer winningPlayer = players[0];
            foreach (var player in players)
            {
                var playerTotalAssetValue = CalculateTotalAssetValue(player, currentStockPrice);
                var winningPlayerTotalAssetValue = CalculateTotalAssetValue(winningPlayer, currentStockPrice);

                if (playerTotalAssetValue > winningPlayerTotalAssetValue)
                {
                    winningPlayer = player;
                }
            }

            return winningPlayer;
        }

        private double CalculateTotalAssetValue(IPLayer player, double currentStockPrice)
        {
            var totalValue = player.MyWallet.Balance + player.MyWallet.Stocks * currentStockPrice;
            return totalValue;
        }

        public string WinningTeamResult { get; set; }

        public string PlayerListResults { get; set; }

        private StockHistory CreateHistoryRecords(int historyCount)
        {
            var stockHistory = new StockHistory();
            stockHistory.Records = new List<double>();
            historyCount = 10;
            for (int i = 0; i < Candles.Count; i++)
            {
                if (i == historyCount)
                    break;

                stockHistory.Records.Add(Candles[i].Close);
            }

            return stockHistory;
        }

        public string CurrentSymbol { get; set; }

        public string Info { get; set; }

        public List<Candle> Candles { get; set; }
        public string WinnerTeam { get; internal set; }

        private StockSymbol GetRandomSymbol(IList<StockSymbol> symbols, Random random)
        {
            var index = random.Next(0, symbols.Count);
            return symbols[index];
        }
    }
}
