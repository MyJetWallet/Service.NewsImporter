using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Service.NewsImporter.Domain;
using Service.NewsImporter.Domain.Models;
using Service.NewsRepository.Domain.Models;
using Service.NewsRepository.Grpc;
using Service.NewsRepository.Grpc.Models;

namespace Service.NewsImporter.Services
{
    public class NewsStorage : INewsStorage
    {
        private readonly INewsService _newsService;
        private readonly ILogger<NewsStorage> _logger;
        public NewsStorage(INewsService newsService, ILogger<NewsStorage> logger)
        {
            _newsService = newsService;
            _logger = logger;
        }

        public async Task SaveNewsAsync(List<ExternalNews> news)
        {
            foreach (var e in news)
            {
                var duplicateDateExists = news.Any(x => x.Date == e.Date && x.NewsUrl != e.NewsUrl);

                if (duplicateDateExists)
                {
                    e.Date = e.Date.AddMilliseconds(1);
                }
            }
            
            var internalNews = news.Select(e => new News()
            {
                AssociatedAssets = e.InternalTickers,
                Source = e.Source,
                Lang = e.Lang,
                Timestamp = e.Date,
                Topic = e.Title,
                Description = e.Description,
                UrlAddress = e.NewsUrl,
                ImageUrl = e.ImageUrl,
                Sentiment = e.Sentiment,
                IntegrationSource = e.IntegrationSource
            });

            internalNews = internalNews.Where(e => !string.IsNullOrWhiteSpace(e.Topic));
            var newsToExecute = internalNews.ToList();

            _logger.LogInformation("Import new is done. Count: {count}", newsToExecute.Count);
            
            await _newsService.AddOrUpdateNewsCollection(new AddOrUpdateNewsCollectionRequest()
            {
                NewsCollection = newsToExecute
            });
        }
    }
}