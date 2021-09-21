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
        private readonly ICryptoPanicImporter _cryptoPanicImporter;

        public ExternalNewsImporter(IStockNewsImporter stockNewsImporter,
            ICryptoPanicImporter cryptoPanicImporter)
        {
            _stockNewsImporter = stockNewsImporter;
            _cryptoPanicImporter = cryptoPanicImporter;
        }

        public async Task<List<ExternalNews>> GetNewsAsync(IEnumerable<string> tickers, bool ignoreLastImportedDate = false)
        {
            var news = new List<ExternalNews>();

            var stockNews = await _stockNewsImporter.GetNewsAsync(tickers, ignoreLastImportedDate);
            if (stockNews != null && stockNews.Any())
                news.AddRange(stockNews);

            var cryptoPanicNews = await _cryptoPanicImporter.GetNewsAsync(tickers, ignoreLastImportedDate);
            if (cryptoPanicNews != null && cryptoPanicNews.Any())
                news.AddRange(cryptoPanicNews);
            
            return news;
        }
    }
}