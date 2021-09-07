using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Service.NewsImporter.Domain;
using Service.NewsImporter.Domain.Models;
using Service.NewsRepository.Domain.Models;
using Service.NewsRepository.Grpc;

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
                Console.WriteLine(JsonConvert.SerializeObject(e));
            }
            
            var internalNews = news.Select(e => new News()
            {
                AssociatedAssets = e.InternalTickers,
                Lang = "en",
                Source = e.Source,
                Timestamp = e.Date,
                Topic = e.Title,
                UrlAddress = e.NewsUrl,
                ImageUrl = e.ImageUrl,
                Sentiment = e.Sentiment
            });
            foreach (var e in internalNews)
            {
                await _newsService.AddOrUpdateNews(e);
            }
        }
    }
}