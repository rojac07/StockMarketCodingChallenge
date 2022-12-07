using DomainModels;
using StockMarketCodingChallengeWpfApp.Converters;
using System.Collections.Generic;
using System.IO;

namespace StockMarketCodingChallengeWpfApp.Repositories
{
    public class StockDataFileRepository : IStockDataFileRepository
    {
        private const string localDir = "StockData";

        private string FilePath(string symbol) => localDir + "//" + symbol + ".csv";
        public List<Candle> Get(string symbol)
        {
            if (!Directory.Exists(localDir))
                return null;
            string filePath = FilePath(symbol);

            if (!File.Exists(filePath))
                return null;

            var content = File.ReadAllText(filePath);
            return CandleConverter.RawDataToCanles(content, symbol);
        }

        public void Save(string symbol, List<Candle> candles)
        {
            if (candles == null || candles.Count <= 1)
                return;

            string filePath = FilePath(symbol);
            if (File.Exists(filePath))
                return;

            var content = CandleConverter.CandlesToRawData(candles);
            if (!Directory.Exists(localDir))
                Directory.CreateDirectory(localDir);
            File.WriteAllText(filePath, content);
        }
    }
}
