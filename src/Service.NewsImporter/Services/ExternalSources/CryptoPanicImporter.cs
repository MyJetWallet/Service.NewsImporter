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
    public class CryptoPanicImporter : ICryptoPanicImporter
    {
        private readonly ILogger<CryptoPanicImporter> _logger;
        
        private static readonly string ApiUrl = Program.Settings.CryptoPanicApiUrl;
        private static readonly string Token = Program.Settings.CryptoPanicToken;
        private static readonly string Regions = Program.Settings.CryptoPanicRegions;
        
        private static readonly HttpClient Client = new HttpClient();

        public CryptoPanicImporter(ILogger<CryptoPanicImporter> logger)
        {
            _logger = logger;
        }

        private DateTime? LastImportedNews { get; set; }
        
        public async Task<List<ExternalNews>> GetNewsAsync(List<ExternalTickerSettings> tickers,
            bool ignoreLastImportedDate = false)
        {
            var newsFromAllPagesAndRegions = new List<ExternalNews>();
            foreach (var ticker in tickers.Where(e => e.IntegrationSource == "CryptoPanic").Select(e => e.NewsTicker))
            {
                foreach (var region in Regions.Trim().Split(","))
                {
                    var requestUrl = GetRequestUrl(region, ticker);
                
                    var nextIsEmpty = false;
            
                    while (!nextIsEmpty)
                    {
                        var nextUrlAndNews = await GetNextUrlAndNewsByUrl(requestUrl);

                        if (nextUrlAndNews.Item1 == null || string.IsNullOrWhiteSpace(nextUrlAndNews.Item1))
                        {
                            nextIsEmpty = true;
                        }
                        else
                        {
                            requestUrl = nextUrlAndNews.Item1;
                        }
                        newsFromAllPagesAndRegions.AddRange(nextUrlAndNews.Item2);
                    }
                }
            }

            var filteredNews = new List<ExternalNews>();
            foreach (var ticker in tickers.Where(e => e.IntegrationSource == "CryptoPanic").Select(e => e.NewsTicker))
            {
                var newsByTicker = newsFromAllPagesAndRegions.Where(e => e.ExternalTickers.Contains(ticker));
                filteredNews.AddRange(newsByTicker);
            }
            filteredNews = filteredNews.Distinct().ToList();

            if (LastImportedNews != null && !ignoreLastImportedDate)
            {
                filteredNews = filteredNews.Where(e => e.Date > LastImportedNews).ToList();
            }
            if (filteredNews.Any())
            {
                LastImportedNews = filteredNews.Max(e => e.Date);
            }
            return filteredNews;
        }
        private async Task<(string, List<ExternalNews>)> GetNextUrlAndNewsByUrl(string requestUrl)
        {
            _logger.LogInformation($"GetNextUrlAndNewsByUrl started with url : {requestUrl}");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUrl),
                Headers =
                {
                    {"Accept", "application/json"}
                }
            };
            await Task.Delay(Program.Settings.CryptoPanicDelayInMs);
            using var response = await Client.SendAsync(request);
            var cryptoPanicApiResponse = new CryptoPanicApiResponse();
            try
            {
                var body = await response.Content.ReadAsStringAsync();
                
                if (response.StatusCode != HttpStatusCode.OK || string.IsNullOrWhiteSpace(body))
                {
                    _logger.LogWarning($"Cannot get news from crypto panic. Code: {response.StatusCode}. Content: {body}");
                    return (string.Empty, new List<ExternalNews>());
                }
                
                cryptoPanicApiResponse = JsonConvert.DeserializeObject<CryptoPanicApiResponse>(body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return (string.Empty, new List<ExternalNews>());
            }
            var responseNews = (cryptoPanicApiResponse.next, new List<ExternalNews>());
            if (cryptoPanicApiResponse?.results == null || !cryptoPanicApiResponse.results.Any())
            {
                responseNews.next = string.Empty;
                return responseNews;
            }
            if (cryptoPanicApiResponse?.results != null && cryptoPanicApiResponse.results.Any())
            {
                responseNews.Item2 = cryptoPanicApiResponse.results.Select(e => new ExternalNews()
                    {
                        Date = e.published_at,
                        ImageUrl = string.Empty,
                        NewsUrl = e.url,
                        Sentiment = SentimentConvertor.ConvertSentiment(e.votes),
                        Source = e.source.title,
                        ExternalTickers = e.currencies?.Select(x => x.code).ToList() ?? new List<string>(),
                        Title = e.title,
                        Description = e.metadata?.description,
                        IntegrationSource = "CryptoPanic",
                        Lang = e.source.region
                    }
                ).ToList();
            }
            _logger.LogInformation($"Finded {responseNews.Item2.Count} news by url : {requestUrl}");
            return responseNews;
        }
        private string GetRequestUrl(string region, string ticker)
        {
            var requestUrl = $"{ApiUrl}?auth_token={Token}&regions={region}&currencies={ticker}&metadata=true";
            return requestUrl;
        }
    }

    public class CryptoPanicApiResponse
    {
        public int count { get; set; }
        public string next { get; set; }
        public string previous { get; set; }
        public List<CryptoPanicResultEntity> results { get; set; }
    }

    public class CryptoPanicResultEntity
    {
        public string kind { get; set; }
        public string domain { get; set; }
        public CryptoPanicSource source { get; set; }
        public string title { get; set; }
        public MetadataItem metadata { get; set; }
        public DateTime published_at { get; set; }
        public string slug { get; set; }
        public List<CryptoPanicCurrencies> currencies { get; set; }
        public long id { get; set; }
        public string url { get; set; }
        public DateTime created_at { get; set; }
        public CryptoPanicVotes votes { get; set; }

        public class MetadataItem
        {
            public string description { get; set; }
            public string image { get; set; }
        }
    }

    public class CryptoPanicVotes
    {
        public int negative { get; set; }
        public int positive { get; set; }
        public int important { get; set; }
        public int liked { get; set; }
        public int disliked { get; set; }
        public int lol { get; set; }
        public int toxic { get; set; }
        public int saved { get; set; }
        public int comments { get; set; }
    }

    public class CryptoPanicCurrencies
    {
        public string code { get; set; }
        public string title { get; set; }
        public string slug { get; set; }
        public string url { get; set; }
    }

    public class CryptoPanicSource
    {
        public string title { get; set; }
        public string region { get; set; }
        public string domain { get; set; }
    }
}