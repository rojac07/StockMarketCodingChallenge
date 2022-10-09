using DomainModels;
using GalaSoft.MvvmLight.Command;
using StockMarketCodingChallengeWpfApp.Interfaces;
using StockMarketCodingChallengeWpfApp.Players;
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
        private IList<IPlayer> players = new List<IPlayer>()
        {
            {new Player1(new Wallet(initialBalance)) },
            {new Player2(new Wallet(initialBalance)) },
        };
        private DateTime start = new DateTime(2004, 1, 1);
        private DateTime end = new DateTime(2018, 1, 15);

        public MainWindowViewModel(Size size)
        {
            PauseCommand = new RelayCommand(OnPausedCommand);
            CreateNewChallengeCommand = new RelayCommand(OnNewChallengeCommand);

            this.stockSimulator = new StockSimulator(players, new YahooWebApiService(), size, GameSpeed.Maximum);
            this.stockSimulator.OnNewTradeDayEvent += RefreshUi;
        }

        private void RefreshUi(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Symbol));
            OnPropertyChanged(nameof(Points));
            OnPropertyChanged(nameof(Information));
            OnPropertyChanged(nameof(PlayerList));
        }

        private void OnNewChallengeCommand()
        {
            string symbol = null;// if null then it will find random symbol
            var isInitialized = this.stockSimulator.CreateNewChallenge(start, end, symbol);
            RefreshUi(null, null);
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

        public string PlayerList => stockSimulator.Results.Results;

        public string Information => stockSimulator.Results.Information;

        public List<Point> Points => stockSimulator.GraphPoints;
    }
}
