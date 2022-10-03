using DomainModels;
using System;
using System.Collections.Generic;

namespace YahooFinanceWebApi
{
    public interface IYahooWebApiService
    {
        List<Candle> Download(string symbol, DateTime startDate, DateTime endDate);
    }
}