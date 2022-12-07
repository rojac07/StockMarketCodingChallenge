using DomainModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StockMarketCodingChallengeWpfApp.Helpers
{
    public class StockSymbolRepository : IStockSymbolRepository
    {
        /// <summary>
        /// File that contains a list of stocks. 
        /// NOTE: this file is a hardcode list and not update thus information 
        /// might be not up to date.
        /// </summary>
        private string stockListCsvFile = @"Resources\nasdaq_screener_1664001254530.csv";

        private IList<StockSymbol> stockSymbols;

        public StockSymbolRepository()
        {
            this.Initialize();
        }

        public StockSymbol Get(string symbol) => this.stockSymbols.First(x => x.Name == symbol);

        public IList<StockSymbol> GetAll() => this.stockSymbols;

        public bool IsInitialized() => this.stockSymbols != null;


        /// <summary>
        /// Parse given stock list file
        /// </summary>
        /// <param name="stockListFilePath"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private void Initialize(string stockListFilePath = null)
        {
            if (IsInitialized())
                return;

            if (string.IsNullOrEmpty(stockListFilePath))
                stockListFilePath = this.stockListCsvFile;

            this.stockSymbols = new List<StockSymbol>();
            var text = File.ReadAllText(stockListFilePath);
            var separator = new[] { "\r\n" };
            var lines = text.Split(separator, StringSplitOptions.None);
            lines[0] = string.Empty;// remove first line
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                var symbol = ParseLine(line);
                stockSymbols.Add(symbol);
            }
        }

        private StockSymbol ParseLine(string line)
        {
            var stockSymbol = new StockSymbol();
            var separator = new[] { "," };
            var blocks = line.Split(separator, StringSplitOptions.None);
            stockSymbol.Symbol = blocks[0];
            stockSymbol.Name = blocks[1];
            return stockSymbol;
        }
    }
}
