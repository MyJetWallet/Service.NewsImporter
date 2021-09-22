using System.Collections.Generic;
using System.Threading.Tasks;
using Service.NewsImporter.Domain.Models;

namespace Service.NewsImporter.Domain
{
    public interface IExternalNewsImporter
    {
        Task<List<ExternalNews>> GetNewsAsync(List<ExternalTickerSettings> tickers, bool ignoreLastImportedDate = false);
    }
}