using DomainModels;
using StockMarketCodingChallengeLibrary;
using System;
using System.Collections.Generic;
using YahooFinanceWebApi;

namespace YahooFInanceApiTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();
            p.Run();
            Console.ReadKey();
        }

        private void Run()
        {
            var stockSymbolParser = new StockSymbolParser();
            var stockSymbols = stockSymbolParser.Parse(@"Resources\nasdaq_screener_1664001254530.csv");

            var randomSymbol = GetRandomSymbol(stockSymbols, new Random());
            Console.WriteLine(randomSymbol.Symbol + "\t" + randomSymbol.Name);
            var startDate = new DateTime(2012,1,1);
            var endDate = new DateTime(2012,12,1);
            YahooWebApiService yahooWebApiService = new YahooWebApiService();
            var candles = yahooWebApiService.Download(randomSymbol.Symbol, startDate, endDate);
        }

      

        private StockSymbol GetRandomSymbol(IList<StockSymbol> symbols, Random random)
        {            
            var index = random.Next(0, symbols.Count);
            return symbols[index];
        }
    }
}

