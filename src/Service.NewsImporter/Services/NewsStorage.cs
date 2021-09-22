using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

        public NewsStorage(INewsService newsService)
        {
            _newsService = newsService;
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
                Lang = "en",
                Source = e.Source,
                Timestamp = e.Date,
                Topic = e.Title,
                Description = e.Description,
                UrlAddress = e.NewsUrl,
                ImageUrl = e.ImageUrl,
                Sentiment = e.Sentiment,
                IntegrationSource = e.IntegrationSource
            });

            internalNews = internalNews.Where(e => !string.IsNullOrWhiteSpace(e.Topic));

            await _newsService.AddOrUpdateNewsCollection(new AddOrUpdateNewsCollectionRequest()
            {
                NewsCollection = internalNews.ToList()
            });
        }
    }
}