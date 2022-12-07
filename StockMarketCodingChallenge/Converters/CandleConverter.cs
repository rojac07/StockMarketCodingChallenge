using DomainModels;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace StockMarketCodingChallengeWpfApp.Converters
{
    public static class CandleConverter
    {
        public static string CandlesToRawData(List<Candle> candles)
        {
            string results = string.Empty;
            foreach (var candle in candles)
            {
                results += CandleToString(candle);
            }
            return results;
        }

        public static List<Candle> RawDataToCanles(string rawData, string symbolName)
        {
            List<Candle> candles = new List<Candle>();
            var separator = new[] { "\n" };
            var lines = rawData.Split(separator, StringSplitOptions.None);
            lines[0] = string.Empty; // remove first line

            foreach (var rawLine in lines)
            {
                if (string.IsNullOrEmpty(rawLine))
                    continue;

                Candle candle = ConvertToCandle(rawLine, symbolName);
                candles.Add(candle);
            }
            return candles;
        }

        private static string CandleToString(Candle candle)
        {
            string result = string.Empty;
            var separator = new[] { "," };
            //result = candle.Name + separator;
            result += candle.Date.ToString(CultureInfo.InvariantCulture) + separator[0];
            result += candle.Open.ToString(CultureInfo.InvariantCulture) + separator[0];
            result += candle.High.ToString(CultureInfo.InvariantCulture) + separator[0];
            result += candle.Low.ToString(CultureInfo.InvariantCulture) + separator[0];
            result += candle.Close.ToString(CultureInfo.InvariantCulture) + "\r\n";
            return result;
        }

        private static Candle ConvertToCandle(string rawLine, string symbolName)
        {
            var candle = new Candle();
            var separator = new[] { "," };
            var blocks = rawLine.Split(separator, StringSplitOptions.None);
            candle.Name = symbolName;
            candle.Date = DateTime.Parse(blocks[0], CultureInfo.InvariantCulture);
            candle.Open = double.Parse(blocks[1], CultureInfo.InvariantCulture);
            candle.High = double.Parse(blocks[2], CultureInfo.InvariantCulture);
            candle.Low = double.Parse(blocks[3], CultureInfo.InvariantCulture);
            candle.Close = double.Parse(blocks[4], CultureInfo.InvariantCulture);
            return candle;
        }
    }
}
