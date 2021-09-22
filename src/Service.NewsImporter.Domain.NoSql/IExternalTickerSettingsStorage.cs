using System.Collections.Generic;
using System.Threading.Tasks;
using Service.NewsImporter.Domain.Models;

namespace Service.NewsImporter.Domain.NoSql
{
    public interface IExternalTickerSettingsStorage
    {
        ExternalTickerSettings GetExternalTickerSettings(string ticker);
        List<ExternalTickerSettings> GetExternalTickerSettings();
        Task UpdateExternalTickerSettingsAsync(ExternalTickerSettings settings);
    }
}