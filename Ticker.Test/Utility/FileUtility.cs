using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticker.Domain.Ticker;
using Ticker.Mediator.Http.AlphaVantage.Models;

namespace Ticker.Test.Utility
{
    internal static class FileUtility
    {
        static public Dictionary<DateTime, OHLCV> GetHistoricalData()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AAPL_HistoricalData_2023_10_5.json");
            var json = File.ReadAllText(filePath);
            var jobject = JObject.Parse(json)["Time Series (Daily)"];
            var timeSeries = jobject?.ToObject<Dictionary<string, AlphaVantageOHLCV>>();

            return timeSeries!.ToDictionary(item => DateTime.Parse(item.Key), item => new OHLCV { Close = item.Value.Close });
        }

        static public Dictionary<DateTime, OHLCV> GetBenchmarkData()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NASDAQ_BenchmarkData_2023_10_5.json");
            var json = File.ReadAllText(filePath);
            var jobject = JObject.Parse(json)["Time Series (Daily)"];
            var timeSeries = jobject?.ToObject<Dictionary<string, AlphaVantageOHLCV>>();

            return timeSeries!.ToDictionary(item => DateTime.Parse(item.Key), item => new OHLCV { Close = item.Value.Close });
        }
    }
}
