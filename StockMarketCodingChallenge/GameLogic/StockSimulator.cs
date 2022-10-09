using DomainModels;
using StockMarketCodingChallengeWpfApp.Helpers;
using StockMarketCodingChallengeWpfApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using YahooFinanceWebApi;

namespace StockMarketCodingChallengeWpfApp
{
    public class StockSimulator : IStockSimulator
    {
        int xPos = 0;
        private double min, max, margins, elementCount;
        private readonly Size windowSize;
        private Timer timer;
        private DateTime start, end; // data range stock data will be taken.         
        private readonly GameSpeed gameSpeed;
        private readonly IList<IPlayer> players;
        private readonly IYahooWebApiService yahooWebApiService;
        
        public StockSimulator(IList<IPlayer> players, IYahooWebApiService yahooWebApiService, Size windowSize, GameSpeed gameSpeed)
        {
            this.players = players;
            this.yahooWebApiService = yahooWebApiService;
            this.windowSize = windowSize;
            this.gameSpeed = gameSpeed;
            GraphPoints = new List<Point>();
        }

        public event EventHandler OnNewTradeDayEvent;

        /// <summary>
        /// Finds a random symbol and loads hystorical data.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public bool CreateNewChallenge(DateTime startDate, DateTime endDate, string symbol = null)
        {
            var candles = LoadStockData(startDate, endDate, symbol);
            if (candles == null || !candles.Any())
                return false;

            xPos = 0;
            elementCount = candles.Count;
            this.start = startDate;
            this.end = endDate;
            this.max = Calc.Max(candles);
            this.min = Calc.Min(candles);
            this.timer = new Timer { Interval = (int)gameSpeed };
            this.timer.Stop();
            this.timer.Elapsed += OnElapsed;
            Candles = new Queue<Candle>();
            GraphPoints = new List<Point>();
            candles.ForEach(x => this.Candles.Enqueue(x));//Add all items to queque collection
            History = CreateHistoricalRecords(10);
            return true;
        }

        public List<Point> GraphPoints { get; set; }

        private StockHistory History { get; set; }

        public string Symbol { get; set; }

        public GameResults Results { get; set; } = new GameResults();

        public Queue<Candle> Candles { get; set; } = new Queue<Candle>();

        private StockSymbol GetRandomSymbol(IList<StockSymbol> symbols, Random random)
        {
            var index = random.Next(0, symbols.Count);
            return symbols[index];
        }
                
        public void Start() => this.timer?.Start();

        public void Pause() => this.timer?.Stop();

        public void Stop() => this.timer?.Stop();
        

        private List<Candle> LoadStockData(DateTime startDate, DateTime endDate, string symbol)
        {
            List<Candle> candles;
            if (!string.IsNullOrEmpty(symbol))
            {
                candles = yahooWebApiService.Download(symbol, startDate, endDate);          
                Console.WriteLine(symbol);
                Symbol = symbol;
                return candles;
            }
          
            var random = new Random();  //Load random stock symbol
            StockSymbol randomStockSymbol;
            do
            {
                var stockSymbolList = StockSymbolParser.Parse(@"Resources\nasdaq_screener_1664001254530.csv");
                randomStockSymbol = GetRandomSymbol(stockSymbolList, random);
                var randomSymbol = randomStockSymbol.Symbol;                
                candles = yahooWebApiService.Download(randomSymbol, startDate, endDate);

            } while (candles == null);

            Symbol = randomStockSymbol.Symbol + ": " + randomStockSymbol.Name;
            Console.WriteLine(randomStockSymbol.Symbol + "\t" + randomStockSymbol.Name);
            return candles;
        }

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            if(!this.Candles.Any())
            {
                this.Stop();
                return;
            }
            var candle = Candles.Dequeue();
            AddNewGraphPoint(candle);
            OnNewTradeDay(candle, this.players);
            Results = CalculateGameResults(players, candle);
            OnNewTradeDayEvent?.Invoke(sender, e);
        }

        private void AddNewGraphPoint(Candle candle)
        {
            double scaleFactorY = Calc.GetScaleFactor(windowSize.Height - margins, max);
            double scaleX = Calc.GetScaleFactor(windowSize.Width, elementCount);
            double x = xPos++ * scaleX;
            double y = windowSize.Height + (-1) * scaleFactorY * candle.Open;
            var point = new Point(x, y);
            GraphPoints.Add(point);
        }

        private void OnNewTradeDay(Candle candle, IList<IPlayer> players)
        {
            foreach (var player in players)
            {
                var stockPrice = candle.Open;
                var tradeAction = new TradeAction(player.MyWallet, stockPrice);
                player.OnNewTradeDay(tradeAction);
            }
            History.Records.Add(candle.Open);
        }

        private GameResults CalculateGameResults(IList<IPlayer> players, Candle candle)
        {
            var gameResults = new GameResults();
            double stockPrice = candle.Open;
            var sortedPlayers = players.OrderByDescending(x => Calc.TotalAssetValue(x, stockPrice)).ToList();
            int standing = 1;
            foreach (var player in sortedPlayers)
            {
                var totalAssets = Math.Round(Calc.TotalAssetValue(player, stockPrice), 0);
                var stocks = Math.Round(player.MyWallet.Stocks, 0);
                var result = $"{standing++}. {player.Name}\t Stocks: {stocks}\t Value: {totalAssets} $ \r\n\r\n";
                gameResults.Results += result;
            }
            gameResults.Information = $"{start.ToShortDateString()} - {end.ToShortDateString()}\t{candle.Date.ToShortDateString()}";
            return gameResults;
        }
        
        private StockHistory CreateHistoricalRecords(int historyCount)
        {
            var stockHistory = new StockHistory();
            stockHistory.Records = new List<double>();
            
            for (int i = 0; i < historyCount; i++)
            {
                var stockPrice = Candles.Dequeue();
                stockHistory.Records.Add(stockPrice.Open);
            }
            return stockHistory;
        }       
    }
}