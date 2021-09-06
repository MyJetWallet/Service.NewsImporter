using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Service.NewsImporter.Domain.ExternalSources;
using Service.NewsImporter.Domain.Models;

namespace Service.NewsImporter.Services.ExternalSources
{
    public class StockNewsImporter : IStockNewsImporter
    {
        private static readonly string ApiUrl = Program.Settings.StockNewsApiUrl;
        private static readonly string Token = Program.Settings.StockNewsToken;
        private static readonly int ImportCount = Program.Settings.StockNewsImportCount;
        
        public async Task<List<News>> GetNewsAsync(List<string> tickers)
        {
            var requestUrl = GetRequestUrl(tickers);
            
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUrl),
                Headers =
                {
                    {"Accept", "application/json"}
                }
            };
            using var response = await client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(body))
            {
                return new List<News>();
            }
            var stockNewsApiResponse = JsonConvert.DeserializeObject<StockNewsApiResponse>(body);

            if (stockNewsApiResponse?.data != null && stockNewsApiResponse.data.Any())
            {
                var newsList = stockNewsApiResponse.data.Select(e => new News()
                    {
                        Date = e.date,
                        ImageUrl = e.image_url,
                        NewsUrl = e.news_url,
                        Sentiment = e.sentiment,
                        Source = e.source_name,
                        Tickers = e.tickers,
                        Title = e.text
                    }
                ).ToList();
                return newsList;
            }
            return new List<News>();
        }

        private string GetRequestUrl(IEnumerable<string> tickers)
        {
            var tickersString = string.Empty;
            foreach (var ticker in tickers)
            {
                if (!string.IsNullOrWhiteSpace(tickersString))
                    tickersString += ",";
                
                tickersString += ticker;
            }
            var requestUrl = $"{ApiUrl}?tickers={tickersString}&items={ImportCount}&token={Token}";
            return requestUrl;
        }
    }

    public class StockNewsApiResponse
    {
        public List<StockNewsEntity> data { get; set; }
    }

    public class StockNewsEntity
    {
        public string news_url { get; set; }
        public string image_url { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string source_name { get; set; }
        public DateTime date { get; set; }
        public string sentiment { get; set; }
        public string type { get; set; }
        public List<string> tickers { get; set; }
    }
}