using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.NewsImporter.Domain.ExternalSources;
using Service.NewsImporter.Domain.Models;

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
        
        public async Task<List<ExternalNews>> GetNewsAsync(IEnumerable<string> tickers, bool ignoreLastImportedDate = false)
        {
            var newsFromAllPagesAndRegions = new List<ExternalNews>();
            foreach (var region in Regions.Trim().Split(","))
            {
                var requestUrl = GetRequestUrl(region);
                
                var nextIsEmpty = false;
            
                while (!nextIsEmpty)
                {
                    var nextUrlAndNews = await GetNextUrlAndNewsByUrl(requestUrl);

                    if (nextUrlAndNews.Item1 == null)
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

            var filteredNews = new List<ExternalNews>();
            foreach (var ticker in tickers)
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
            filteredNews = filteredNews.Where(e => !string.IsNullOrWhiteSpace(e.Title)).ToList();
            return filteredNews;
        }
        private async Task<(string, List<ExternalNews>)> GetNextUrlAndNewsByUrl(string requestUrl)
        {
            _logger.LogInformation("Request url is {requestUrl}", requestUrl);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUrl),
                Headers =
                {
                    {"Accept", "application/json"}
                }
            };
            await Task.Delay(300);
            using var response = await Client.SendAsync(request);
            var cryptoPanicApiResponse = new CryptoPanicApiResponse();
            try
            {
                var body = await response.Content.ReadAsStringAsync();
                
                //_logger.LogInformation("Response body is {reponseBody}", body);

                if (string.IsNullOrWhiteSpace(body))
                {
                    return (string.Empty, new List<ExternalNews>());
                }

                cryptoPanicApiResponse = JsonConvert.DeserializeObject<CryptoPanicApiResponse>(body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return (string.Empty, new List<ExternalNews>());
            }
            
            if (cryptoPanicApiResponse?.results == null || !cryptoPanicApiResponse.results.Any())
            {
                var exMessage = "Empty results";
                var ex = new Exception(exMessage);
                _logger.LogError($"Response has body with errors: {exMessage}", ex);
                throw ex;
            }
            var responseNews = (cryptoPanicApiResponse.next, new List<ExternalNews>());
            if (cryptoPanicApiResponse?.results != null && cryptoPanicApiResponse.results.Any())
            {
                responseNews.Item2 = cryptoPanicApiResponse.results.Select(e => new ExternalNews()
                    {
                        Date = e.published_at,
                        ImageUrl = string.Empty,
                        NewsUrl = e.url,
                        Sentiment = string.Empty,
                        Source = e.source.title,
                        ExternalTickers = e.currencies?.Select(x => x.code).ToList() ?? new List<string>(),
                        Title = e.title,
                        Description = string.Empty,
                        IntegrationSource = "CryptoPanic"
                    }
                ).ToList();
            }
            _logger.LogInformation($"Finded {responseNews.Item2.Count} news by url : {requestUrl}");
            return responseNews;
        }
        private string GetRequestUrl(string region)
        {
            var requestUrl = $"{ApiUrl}?auth_token={Token}&regions={region}";
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
        public DateTime published_at { get; set; }
        public string slug { get; set; }
        public List<CryptoPanicCurrencies> currencies { get; set; }
        public long id { get; set; }
        public string url { get; set; }
        public DateTime created_at { get; set; }
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