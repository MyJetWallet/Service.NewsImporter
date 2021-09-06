using MyNoSqlServer.Abstractions;

namespace Service.NewsImporter.Domain.NoSql
{
    public class ExternalTickerSettingsNoSql : MyNoSqlDbEntity
    {
        public const string TableName = "myjetwallet-externalticker-settings";
        private static string GeneratePartitionKey(string ticker) => $"ticker:{ticker}";
        private static string GenerateRowKey() => "settings";
        public ExternalTickerSettings Settings { get; set; }
        
        public static ExternalTickerSettingsNoSql Create(ExternalTickerSettings settings)
        {
            return new ExternalTickerSettingsNoSql()
            {
                PartitionKey = GeneratePartitionKey(settings.NewsTicker),
                RowKey = GenerateRowKey(),
                Settings = settings
            };
        }
    }
}