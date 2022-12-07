using DomainModels;
using GalaSoft.MvvmLight.Command;
using StockMarketCodingChallengeWpfApp.Helpers;
using StockMarketCodingChallengeWpfApp.Interfaces;
using StockMarketCodingChallengeWpfApp.Players;
using StockMarketCodingChallengeWpfApp.Repositories;
using System;
using System.Collections.Generic;
using System.Windows;
using YahooFinanceWebApi;

namespace StockMarketCodingChallengeWpfApp
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly StockSimulator stockSimulator;
        private static readonly double initialBalance = 10000;
        private bool toggle = true;
        private Size windowSize;
        int xPos = 0;
        private IList<Tuple<IPlayer, Wallet>> players = new List<Tuple<IPlayer, Wallet>>()
        {
            {new Tuple<IPlayer, Wallet>( new Player1(), new Wallet(initialBalance)) },
            {new Tuple<IPlayer, Wallet>( new Player2(), new Wallet(initialBalance)) },//
            
            //..
        };
        private DateRange dateRange = new DateRange
        {
            StartDate = new DateTime(2004, 1, 1),
            EndDate = new DateTime(2010, 1, 1)
        };


        public MainWindowViewModel(Size size)
        {
            PauseCommand = new RelayCommand(OnPausedCommand);
            CreateNewChallengeCommand = new RelayCommand(OnNewChallengeCommand);

            this.stockSimulator = new StockSimulator(players, new YahooWebApiService(), new StockSymbolRepository(), new StockDataFileRepository(), GameSpeed.Maximum);
            this.stockSimulator.OnNewTradeDayEvent += RefreshUi;
            this.windowSize = size;
        }

        private void RefreshUi(object sender, EventArgs e)
        {
            Candle candle = (Candle)sender;
            AddNewPoint(candle, 0, 100, 10000);// TODO: n´calculate correct values..
            CurrentStockPrice = candle.Open;
            OnPropertyChanged(nameof(CurrentStockPrice));
            OnPropertyChanged(nameof(Symbol));
            OnPropertyChanged(nameof(Points));
            OnPropertyChanged(nameof(Information));
            OnPropertyChanged(nameof(PlayerList));
        }

        private void OnNewChallengeCommand()
        {
            //var isInitialized = this.stockSimulator.NewGame(dateRange, "SYMBOL NAME");
            var isInitialized = this.stockSimulator.NewGame(dateRange, 10);
        }

        private void AddNewPoint(Candle candle, int minPrice, int maxPrice, int elementCount)
        {
            int margins = 10;
            double scaleFactorY = Calc.ScaleFactor(windowSize.Height - margins, maxPrice);
            double scaleX = Calc.ScaleFactor(windowSize.Width, elementCount);
            double x = xPos++ * scaleX;
            double y = windowSize.Height + (-1) * scaleFactorY * candle.Open;
            var point = new Point(x, y);
            Points.Add(point);
        }

        private void OnPausedCommand()
        {
            if (toggle)
                stockSimulator.Start();
            else
                stockSimulator.Stop();

            toggle = !toggle;
        }

        public RelayCommand PauseCommand { get; set; }

        public RelayCommand CreateNewChallengeCommand { get; set; }

        public string Symbol => stockSimulator.Symbol;

        public double CurrentStockPrice { get; set; }

        public string PlayerList => Calc.GetSortedPLayerList(players, this.CurrentStockPrice);

        public string Information => "stockSimulator.Results.Information";

        public List<Point> Points { get; set; } = new List<Point>();
    }
}
