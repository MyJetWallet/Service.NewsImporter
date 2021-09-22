using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Logging;
using MyNoSqlServer.Abstractions;
using Newtonsoft.Json;
using Service.NewsImporter.Domain.Models;
using Service.NewsImporter.Domain.NoSql;

namespace Service.NewsImporter.Services
{
    public class ExternalTickerSettingsStorage : IExternalTickerSettingsStorage, IStartable
    {
        
        private readonly ILogger<ExternalTickerSettingsStorage> _logger;
        private readonly IMyNoSqlServerDataWriter<ExternalTickerSettingsNoSql> _settingsDataWriter;
        
        private Dictionary<string, ExternalTickerSettings> _settings = new Dictionary<string, ExternalTickerSettings>();

        public ExternalTickerSettingsStorage(ILogger<ExternalTickerSettingsStorage> logger,
            IMyNoSqlServerDataWriter<ExternalTickerSettingsNoSql> settingsDataWriter)
        {
            _logger = logger;
            _settingsDataWriter = settingsDataWriter;
        }

        public ExternalTickerSettings GetExternalTickerSettings(string ticker)
        {
            return _settings.TryGetValue(ticker, out var result) ? result : null;
        }

        public List<ExternalTickerSettings> GetExternalTickerSettings()
        {
            return _settings.Values.ToList();
        }

        public async Task UpdateExternalTickerSettingsAsync(ExternalTickerSettings settings)
        {
            await _settingsDataWriter.InsertOrReplaceAsync(ExternalTickerSettingsNoSql.Create(settings));

            await ReloadSettings();

            _logger.LogInformation("Updated ExternalTickerSettings Settings: {jsonText}",
                JsonConvert.SerializeObject(settings));
        }

        public void Start()
        {
            ReloadSettings().GetAwaiter().GetResult();
        }

        private async Task ReloadSettings()
        {
            var settings = (await _settingsDataWriter.GetAsync()).ToList();

            var settingsMap = new Dictionary<string, ExternalTickerSettings>();
            foreach (var elem in settings)
            {
                settingsMap[elem.Settings.NewsTicker] =
                    elem.Settings;
            }
            _settings = settingsMap;
        }
    }
}