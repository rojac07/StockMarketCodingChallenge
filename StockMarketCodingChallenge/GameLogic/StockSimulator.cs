using DomainModels;
using StockMarketCodingChallengeWpfApp.Helpers;
using StockMarketCodingChallengeWpfApp.Interfaces;
using StockMarketCodingChallengeWpfApp.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using YahooFinanceWebApi;

namespace StockMarketCodingChallengeWpfApp
{
    public class StockSimulator : IStockSimulator
    {
        int xPos = 0;
        private double minPrice, maxPrice, elementCount;
        private Timer timer;
        private DateRange dateRange; // data range stock data will be taken.         
        private readonly GameSpeed gameSpeed;
        private readonly IList<Tuple<IPlayer, Wallet>> players;
        private readonly IYahooWebApiService yahooWebApiService;
        private readonly IStockSymbolRepository stockSymbolRepository;
        private readonly IStockDataFileRepository stockDataFileRepository;
        public event EventHandler OnNewTradeDayEvent;


        public StockSimulator(IList<Tuple<IPlayer, Wallet>> players,
                              IYahooWebApiService yahooWebApiService,
                              IStockSymbolRepository stockSymbolRepository,
                              IStockDataFileRepository stockDataFileRepository,
                              GameSpeed gameSpeed)
        {
            this.players = players;
            this.yahooWebApiService = yahooWebApiService;
            this.stockSymbolRepository = stockSymbolRepository;
            this.stockDataFileRepository = stockDataFileRepository;
            this.gameSpeed = gameSpeed;
        }

        /// <summary>
        /// Finds a random symbol and loads hystorical data.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public bool NewGame(DateRange dateRange, string symbol = null)
        {
            var candles = LoadStockData(dateRange, symbol);
            if (candles == null || !candles.Any())
                return false;

            xPos = 0;
            elementCount = candles.Count;
            this.dateRange = dateRange;

            this.timer = new Timer { Interval = (int)gameSpeed };
            this.timer.Stop();
            this.timer.Elapsed += OnElapsed;
            this.Candles = new Queue<Candle>();
            candles.ForEach(x => this.Candles.Enqueue(x));//Add all items to queque collection
            History = CreateHistoricalRecords(30);
            return true;
        }

        private void Initialize(double balance)
        {
            foreach (var item in this.players)
            {
                item.Item2.Balance = balance;
                item.Item2.Stocks = 0;
            }

            History = new StockHistory
            {
                Records = new List<double>()
            };
            Symbol = string.Empty;
            Candles = new Queue<Candle>();
        }

        public bool NewGame(DateRange dateRange, int randomStockListCount)
        {
            var random = new Random();
            AllResults = new List<GameResults>();
            var stockSymbolList = stockSymbolRepository.GetAll();

            for (int i = 0; i < randomStockListCount; i++)
            {
                Initialize(10000);

                var symbol = GetRandomSymbol(stockSymbolList, random);
                var candles = LoadStockData(dateRange, symbol.Symbol);

                if (candles == null || candles.Count() < 30)
                    return false;
                
                candles.ForEach(x => this.Candles.Enqueue(x));//Add all items to queque collection
                History = CreateHistoricalRecords(30);
                var gameResults = PlayGame(candles, players);
                AllResults.Add(gameResults);
            }

            return true;
        }

        public List<GameResults> AllResults { get; private set; }
        private GameResults PlayGame(List<Candle> candles, IList<Tuple<IPlayer, Wallet>> players)
        {
            Candle candle = null;
            foreach (var c in Candles)
            {
                foreach (var player in players)
                {
                    var stockPrice = c.Open;
                    var wallet = player.Item2;
                    var tradeAction = new TradeAction(wallet, stockPrice);
                    //Debug.Write(player.Item1.Name + ": ");
                    player.Item1.OnNewTradeDay(tradeAction, wallet, History.Records);
                }
                History.Records.Add(c.Open);
                candle = c;
            }
            var results = CalculateGameResults(players, candle.Open);
            Debug.WriteLine(Calc.GetSortedPLayerList(players, candle.Open));


            return results;
        }

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


        private List<Candle> LoadStockData(DateRange dateRange, string symbol)
        {
            List<Candle> candles;
            if (!string.IsNullOrEmpty(symbol))
            {
                candles = stockDataFileRepository.Get(symbol);//Try get first from local cache.. 
                if (candles == null)
                    candles = yahooWebApiService.Download(symbol, dateRange.StartDate, dateRange.EndDate);

                stockDataFileRepository.Save(symbol, candles);//Save data locally
                Console.WriteLine(symbol);
                Symbol = symbol;

                return candles?.Select(x => x).Where(x => x.Date > dateRange.StartDate).ToList();
            }

            var random = new Random();  //Load random stock symbol
            StockSymbol randomStockSymbol;
            do
            {
                var stockSymbolList = stockSymbolRepository.GetAll();
                randomStockSymbol = GetRandomSymbol(stockSymbolList, random);
                var randomSymbol = randomStockSymbol.Symbol;

                candles = stockDataFileRepository.Get(randomSymbol);//Try get first from local cache.. 
                if (candles == null)
                    candles = yahooWebApiService.Download(randomSymbol, dateRange.StartDate, dateRange.EndDate);

                stockDataFileRepository.Save(randomSymbol, candles);//Save data locally

            } while (candles == null);

            Symbol = randomStockSymbol.Symbol + ": " + randomStockSymbol.Name;
            Console.WriteLine(randomStockSymbol.Symbol + "\t" + randomStockSymbol.Name);
            return candles;
        }

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            if (!this.Candles.Any())
            {
                this.Stop();
                return;
            }
            var candle = Candles.Dequeue();
            //AddNewGraphPoint(candle);
            OnNewTradeDay(candle, this.players);
            Results = CalculateGameResults(players, candle.Open);
            OnNewTradeDayEvent?.Invoke(candle, e);
        }

        private void OnNewTradeDay(Candle candle, IList<Tuple<IPlayer, Wallet>> players)
        {
            foreach (var player in players)
            {
                var stockPrice = candle.Open;
                var wallet = player.Item2;
                var tradeAction = new TradeAction(wallet, stockPrice);
                player.Item1.OnNewTradeDay(tradeAction, wallet, History.Records);
            }
            History.Records.Add(candle.Open);
        }

        private GameResults CalculateGameResults(IList<Tuple<IPlayer, Wallet>> players, double currentStockPrice)
        {
            var sortedPlayers = players.OrderByDescending(x => Calc.TotalAssetValue(x.Item2, currentStockPrice)).ToList();
            var gameResults = new GameResults
            {
                Players = sortedPlayers
            };
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

        public void SaveResults()
        {
            throw new NotImplementedException();
        }


    }
}