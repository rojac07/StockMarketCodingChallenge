using DomainModels;
using System;
using System.Collections.Generic;
using System.IO;

namespace StockMarketCodingChallengeWpfApp.Helpers
{
    public static class StockSymbolParser
    {
        public static List<StockSymbol> Parse(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("Given filePath cannot be null or empty!");
            }

            var stockSymbols = new List<StockSymbol>();
            var text = File.ReadAllText(filePath);
            var separator = new[] { "\r\n" };
            var lines = text.Split(separator, StringSplitOptions.None);
            lines[0] = string.Empty;//remove first line
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                var symbol = ParseLine(line);
                stockSymbols.Add(symbol);
            }
            return stockSymbols;
        }

        private static StockSymbol ParseLine(string line)
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
