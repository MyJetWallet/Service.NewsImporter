using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service.NewsImporter.Domain;
using Service.NewsImporter.Domain.ExternalSources;
using Service.NewsImporter.Domain.Models;

namespace Service.NewsImporter.Services
{
    public class ExternalNewsImporter : IExternalNewsImporter
    {
        private readonly IStockNewsImporter _stockNewsImporter;

        public ExternalNewsImporter(IStockNewsImporter stockNewsImporter)
        {
            _stockNewsImporter = stockNewsImporter;
        }

        public async Task<List<News>> GetNewsAsync(List<string> tickers)
        {
            var news = new List<News>();

            var stockNews = await _stockNewsImporter.GetNewsAsync(tickers);
            if (stockNews != null && stockNews.Any())
                news.AddRange(stockNews);

            return news;
        }
    }
}