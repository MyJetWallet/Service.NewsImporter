using System.Collections.Generic;

namespace Service.NewsImporter.Domain.NoSql
{
    public class ExternalTickerSettings
    {
        public string NewsTicker { get; set; }
        public List<string> AssociateSymbols { get; set; }
    }
}