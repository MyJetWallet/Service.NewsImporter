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
        public int NewsImportTimerInSec { get; set; }

        [YamlProperty("NewsImporter.StockNewsApiUrl")]
        public string StockNewsApiUrl { get; set; }

        [YamlProperty("NewsImporter.StockNewsToken")]
        public string StockNewsToken { get; set; }

        [YamlProperty("NewsImporter.StockNewsImportCount")]
        public int StockNewsImportCount { get; set; }

        [YamlProperty("NewsImporter.NewsRepositoryGrpcServiceUrl")]
        public string NewsRepositoryGrpcServiceUrl { get; set; }

        [YamlProperty("NewsImporter.MyNoSqlWriterUrl")]
        public string MyNoSqlWriterUrl { get; set; }

        [YamlProperty("NewsImporter.CryptoPanicApiUrl")]
        public string CryptoPanicApiUrl { get; set; }
        [YamlProperty("NewsImporter.CryptoPanicToken")]
        public string CryptoPanicToken { get; set; }
        [YamlProperty("NewsImporter.CryptoPanicRegions")]
        public string CryptoPanicRegions { get; set; }
    }
}
