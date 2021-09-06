using System.Threading.Tasks;
using Service.NewsImporter.Domain;

namespace Service.NewsImporter.Services
{
    public class NewsImportManager : INewsImportManager
    {
        private readonly ITickerStorage _tickerStorage;
        private readonly IExternalNewsImporter _externalNewsImporter;
        private readonly INewsStorage _newsStorage;

        public NewsImportManager(ITickerStorage tickerStorage,
            IExternalNewsImporter externalNewsImporter,
            INewsStorage newsStorage)
        {
            _tickerStorage = tickerStorage;
            _externalNewsImporter = externalNewsImporter;
            _newsStorage = newsStorage;
        }

        public async Task HandleNewsAsync()
        {
            var tickers = await _tickerStorage.GetTickers();
            var news = await _externalNewsImporter.GetNewsAsync(tickers);
            await _newsStorage.SaveNewsAsync(news);
        }
    }
}