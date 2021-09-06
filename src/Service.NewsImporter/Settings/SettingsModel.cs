using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace Service.NewsImporter.Settings
{
    public class SettingsModel
    {
        [YamlProperty("NewsImporter.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }

        [YamlProperty("NewsImporter.ZipkinUrl")]
        public string ZipkinUrl { get; set; }

        [YamlProperty("NewsImporter.ElkLogs")]
        public LogElkSettings ElkLogs { get; set; }

        [YamlProperty("NewsImporter.NewsImportTimerInSec")]
        public double NewsImportTimerInSec { get; set; }
    }
}
