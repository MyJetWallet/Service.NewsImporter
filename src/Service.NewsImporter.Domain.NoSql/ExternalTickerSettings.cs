using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Service.NewsImporter.Domain.NoSql
{
    [DataContract]
    public class ExternalTickerSettings
    {
        [DataMember(Order = 1)] public string NewsTicker { get; set; }
        [DataMember(Order = 2)] public List<string> AssociateSymbols { get; set; }
    }
}