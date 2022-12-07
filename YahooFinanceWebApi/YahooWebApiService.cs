using DomainModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;

namespace YahooFinanceWebApi
{
    public class YahooWebApiService : IYahooWebApiService
    {
        private string queryV7 = "https://query1.finance.yahoo.com/v7/finance/download/{0}?period1={1}&period2={2}&interval=1d&events=history&includeAdjustedClose=true";
        //https://query1.finance.yahoo.com/v7/finance/download/DFIN?period1=0&period2=1669420800&interval=1d&events=history&includeAdjustedClose=true

        public List<Candle> Download(string symbol, DateTime startDate, DateTime endDate)
        {
            if (string.IsNullOrEmpty(symbol))
                throw new ArgumentNullException("Symbol cannot be empty or null!");
            
            return DownloadData(symbol, startDate, endDate);
        }

        private List<Candle> DownloadData(string symbol, DateTime startDate, DateTime endDate)
        {
            List<Candle> candles = null;
            //startDate == null ? "0" : ConvertToUnixTimestamp(startDate).ToString(); // if given 'null', then it will try return MAX period.
            string start = "1";// 1-means that it will pick up min date that is available
            string url = string.Format(queryV7,
                                       symbol,
                                       start,
                                       ConvertToUnixTimestamp(endDate));

            HttpWebRequest request = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            request.Timeout = 10 * 1000;// 10 seconds
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
                Debug.WriteLine("Ex: " + ex?.Message);
                return null;
            }
        }

        /// <summary>
        /// Converts given date to UnixTimestamp. 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private static double ConvertToUnixTimestamp(DateTime date)
        {
            //Option 1
            //DateTime origin = new DateTime(1970, 1, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            //TimeSpan diff = date.ToUniversalTime() - origin;
            //return Math.Floor(diff.TotalSeconds);

            //Option 2
            // Get the offset from current time in UTC time
            DateTimeOffset dto = new DateTimeOffset(DateTime.UtcNow);
            // Get the unix timestamp in seconds
            var unixTime = dto.ToUnixTimeSeconds();
            return unixTime;
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
            candle.Open = double.Parse(blocks[1], CultureInfo.InvariantCulture);
            candle.High = double.Parse(blocks[2], CultureInfo.InvariantCulture);
            candle.Low = double.Parse(blocks[3], CultureInfo.InvariantCulture);
            candle.Close = double.Parse(blocks[4], CultureInfo.InvariantCulture);
            return candle;
        }
    }
}
