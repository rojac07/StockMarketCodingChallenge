﻿using System.Windows;

namespace StockMarketCodingChallengeWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();            
            InitializeComponent();
        }
    }
}
