using DomainModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace YahooFinanceWebApi
{
    public class YahooWebApiService : IYahooWebApiService
    {
        private string query = "https://query1.finance.yahoo.com/v7/finance/download/{0}?period1={1}&period2={2}&interval=1d&events=history&includeAdjustedClose=true";

        public List<Candle> Download(string symbol, DateTime startDate, DateTime endDate)
        {
            return DownloadData(symbol, startDate, endDate);
        }

        private List<Candle> DownloadData(string symbol, DateTime startDate, DateTime endDate)
        {
            List<Candle> candles = null;
            string url = string.Format(query,
                                       symbol,
                                       ConvertToUnixTimestamp(startDate),
                                       ConvertToUnixTimestamp(endDate));

            HttpWebRequest request = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            request.Timeout = 10 * 1000;
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader input = new StreamReader(response.GetResponseStream()))
                    {
                        var rawData = input.ReadToEnd();
                        candles = Parse(rawData, symbol);
                    }
                }
                return candles;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        private static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        private List<Candle> Parse(string rawData, string symbolName)
        {
            List<Candle> candles = new List<Candle>();
            var separator = new[] { "\n" };
            var lines = rawData.Split(separator, StringSplitOptions.None);
            lines[0] = string.Empty; // remove first line

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                Candle candle = ParseLine(line, symbolName);
                candles.Add(candle);
            }

            return candles;
        }

        private Candle ParseLine(string line, string symbolName)
        {
            var candle = new Candle();
            var separator = new[] { "," };
            var blocks = line.Split(separator, StringSplitOptions.None);
            candle.Name = symbolName;
            candle.Date = DateTime.Parse(blocks[0]);
            candle.Open = double.Parse(blocks[1]);
            candle.High = double.Parse(blocks[2]);
            candle.Low = double.Parse(blocks[3]);
            candle.Close = double.Parse(blocks[4]);
            return candle;
        }
    }
}
