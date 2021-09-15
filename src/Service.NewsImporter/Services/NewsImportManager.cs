using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Service.NewsImporter.Domain;
using Service.NewsImporter.Domain.Models;
using Service.NewsImporter.Domain.NoSql;

namespace Service.NewsImporter.Services
{
    public class NewsImportManager : INewsImportManager
    {
        private readonly IExternalNewsImporter _externalNewsImporter;
        private readonly INewsStorage _newsStorage;
        private readonly IExternalTickerSettingsStorage _externalTickerSettingsStorage;
        private readonly ILogger<NewsImportManager> _logger;

        public NewsImportManager(IExternalNewsImporter externalNewsImporter,
            INewsStorage newsStorage,
            IExternalTickerSettingsStorage externalTickerSettingsStorage,
            ILogger<NewsImportManager> logger)
        {
            _externalNewsImporter = externalNewsImporter;
            _newsStorage = newsStorage;
            _externalTickerSettingsStorage = externalTickerSettingsStorage;
            _logger = logger;
        }

        public async Task HandleNewsAsync()
        {
            var externalTickers = _externalTickerSettingsStorage
                .GetExternalTickerSettings()
                .Select(e => e.NewsTicker)
                .ToList();
            
            if (externalTickers.Any())
            {
                var news = await _externalNewsImporter.GetNewsAsync(externalTickers);

                SwapTickers(news);
                
                await _newsStorage.SaveNewsAsync(news);
                
                _logger.LogInformation("Import new is done. Count: {count}", news.Count);
            }
        }

        private void SwapTickers(List<ExternalNews> newsList)
        {
            foreach (var news in newsList)
            {
                var internalTickers = new List<string>();
                foreach (var externalTicker in news.ExternalTickers)
                {
                    var tickerSettings = _externalTickerSettingsStorage.GetExternalTickerSettings(externalTicker);
                    if (tickerSettings?.AssociateSymbols != null && tickerSettings.AssociateSymbols.Any())
                        internalTickers.AddRange(tickerSettings.AssociateSymbols);
                }
                internalTickers = internalTickers.Distinct().ToList();
                news.InternalTickers = internalTickers;
            }
        }
    }
}