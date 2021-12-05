using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.NewsImporter.Domain.ExternalSources;
using Service.NewsImporter.Domain.Models;
using Service.NewsImporter.Domain.NoSql;

namespace Service.NewsImporter.Services.ExternalSources
{
    public class StockNewsImporter : IStockNewsImporter
    {
        private readonly ILogger<StockNewsImporter> _logger;
        
        private static readonly string ApiUrl = Program.Settings.StockNewsApiUrl;
        private static readonly string Token = Program.Settings.StockNewsToken;
        private static int ImportCount = Program.Settings.StockNewsImportCount;

        private static readonly HttpClient Client = new HttpClient();

        public StockNewsImporter(ILogger<StockNewsImporter> logger)
        {
            _logger = logger;
        }

        private DateTime? LastImportedNews { get; set; }

        public async Task<List<ExternalNews>> GetNewsAsync(List<ExternalTickerSettings> tickers,
            bool ignoreLastImportedDate = false)
        {
            if (LastImportedNews != null)
            {
                var requestUrl = GetRequestUrl(tickers.Where(e => e.IntegrationSource == "StockNews").Select(e => e.NewsTicker));
                var news = await GetNewsByUrl(requestUrl);

                if (!ignoreLastImportedDate)
                {
                    news = news.Where(e => e.Date > LastImportedNews).ToList();
                }
                if (news.Any())
                {
                    LastImportedNews = news.Max(e => e.Date);
                }
                return news;
            }
            var responseNews = new List<ExternalNews>();
            foreach (var ticker in tickers.Where(e => e.IntegrationSource == "StockNews").Select(e => e.NewsTicker))
            {
                var requestUrlByOneTicker = GetRequestUrl(new List<string>(){ticker});
                var newsByOneTicker = await GetNewsByUrl(requestUrlByOneTicker);
                responseNews.AddRange(newsByOneTicker);
            }
            if (responseNews.Any())
            {
                LastImportedNews = responseNews.Max(e => e.Date);
            }
            return responseNews;
        }

        private async Task<List<ExternalNews>> GetNewsByUrl(string requestUrl)
        {
            if (string.IsNullOrWhiteSpace(requestUrl))
            {
                return new List<ExternalNews>();
            }
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUrl),
                Headers =
                {
                    {"Accept", "application/json"}
                }
            };
            using var response = await Client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            
            if (string.IsNullOrWhiteSpace(body) || response.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogWarning($"Cannot get news from StockNews, code: {response.StatusCode}, content: {body}");
                return new List<ExternalNews>();
            }
            var stockNewsApiResponse = JsonConvert.DeserializeObject<StockNewsApiResponse>(body);

            if (!string.IsNullOrWhiteSpace(stockNewsApiResponse.message))
            {
                var exMessage = stockNewsApiResponse.message;
                foreach (var error in stockNewsApiResponse.errors.items)
                {
                    exMessage += "\n" + error;
                }
                var ex = new Exception(exMessage);
                _logger.LogError($"Response has body with errors: {exMessage}", ex);
                throw ex;
            }
            var responseNews = new List<ExternalNews>();
            if (stockNewsApiResponse?.data != null && stockNewsApiResponse.data.Any())
            {
                responseNews = stockNewsApiResponse.data.Select(e => new ExternalNews()
                    {
                        Date = e.date,
                        ImageUrl = e.image_url,
                        NewsUrl = e.news_url,
                        Sentiment = e.sentiment,
                        Source = e.source_name,
                        ExternalTickers = e.tickers,
                        Title = e.title,
                        Description = e.text,
                        IntegrationSource = "StockNews",
                        Lang = "en"
                    }
                ).ToList();
            }
            _logger.LogInformation($"Finded {responseNews.Count} news by url : {requestUrl}");
            return responseNews;
        }

        private string GetRequestUrl(IEnumerable<string> tickers)
        {
            var tickersString = string.Empty;
            if (!tickers.Any())
            {
                return string.Empty;
            }
            foreach (var ticker in tickers)
            {
                if (!string.IsNullOrWhiteSpace(tickersString))
                    tickersString += ",";
                
                tickersString += ticker;
            }

            if (ImportCount > 50)
            {
                _logger.LogError("StockNewsImportCount cannot be more than 50");
                ImportCount = 50;
            }
            
            var requestUrl = $"{ApiUrl}?tickers={tickersString}&items={ImportCount}&token={Token}";
            return requestUrl;
        }
    }

    public class StockNewsApiResponse
    {
        public List<StockNewsEntity> data { get; set; }
        public string message { get; set; }
        public StockNewsApiError errors { get; set; }
    }

    public class StockNewsApiError
    {
        public List<string> items { get; set; }
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