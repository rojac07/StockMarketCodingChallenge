using DomainModels;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;

namespace StockMarketCodingChallengeWpfApp
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly StockMarketRunner runner;
        private IList<IPLayer> players;
        private DateTime start = new DateTime(2006, 1, 1);
        private DateTime end = new DateTime(2014, 1, 15);
        public MainWindowViewModel()
        {
            this.runner = new StockMarketRunner();
            this.players = new List<IPLayer>();
            players.Add(new Players.Player1(new Wallet(10000)));
            players.Add(new Players.Player2(new Wallet(10000)));

            runner.Initialize(start, end, players);
            Initialize();
        }

        private void Initialize()
        {
            Timer timerOnAction = new Timer
            {
                Interval = 1000
            };
            timerOnAction.Elapsed += TimerOnAction;
            timerOnAction.Start();

            Timer timerDrawLine = new Timer
            {
                Interval = 100
            };
            timerDrawLine.Elapsed += TimerDrawLine;
            timerDrawLine.Start();
        }

        private void TimerDrawLine(object sender, ElapsedEventArgs e)
        {
            DrawMultiline(i++);
            OnPropertyChanged(nameof(SymbolPoints));
        }

        int i = 10;
        private void TimerOnAction(object sender, System.Timers.ElapsedEventArgs e)
        {
            runner.OnAction(i, this.players);
            OnPropertyChanged(nameof(WinningPlayer));
            OnPropertyChanged(nameof(PlayerList));
            OnPropertyChanged(nameof(SymbolPoints));
            Console.WriteLine("On Elapsed..");
        }

        private double Max(List<Candle> candles)
        {
            double max = 0;
            foreach (var candle in candles)
            {
                if (candle.Close > max)
                    max = candle.Close;
            }
            return max;
        }

        public void DrawMultiline(int i)
        {
            int maxY = 600;
            int middleY = 300 + 50;
            int minY = 0;
            int maxX = 800;

            int max = (int)Max(runner.Candles);
            double scaleY = GetScaleFactor(middleY, max);
            double scaleX = GetScaleFactor(maxX, runner.Candles.Count);
            int x = 0;
            if (runner.Candles.Count <= i + 1)
                return;
            var point = new Point(i * scaleX, middleY + (-1) * scaleY * runner.Candles[i].Close);
            SymbolPoints.Add(point);

            OnPropertyChanged(nameof(SymbolPoints));
            OnPropertyChanged(nameof(Info));
        }



        private double GetScaleFactor(int maxY, int max)
        {
            return (double)maxY / max;
        }

        List<Point> points = new List<Point>();
        public List<Point> SymbolPoints
        {
            get { return points; }
            set
            {
                points = value;
                OnPropertyChanged(nameof(SymbolPoints));
            }
        }

        public string Symbol => runner.CurrentSymbol;
        public string WinningPlayer => runner.WinningTeamResult;
        public string PlayerList => runner.PlayerListResults;
        public string WinnerTeam => runner.WinnerTeam;
        public string Info =>"["+ start.ToShortDateString() + " - " + end.ToShortDateString() + "]  ["  + runner.Info + "]";
    }
}
