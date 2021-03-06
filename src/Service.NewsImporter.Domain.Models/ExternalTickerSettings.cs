using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Service.NewsImporter.Domain.Models
{
    [DataContract]
    public class ExternalTickerSettings
    {
        [DataMember(Order = 1)] public string IntegrationSource { get; set; }
        [DataMember(Order = 2)] public string NewsTicker { get; set; }
        [DataMember(Order = 3)] public List<string> AssociateSymbols { get; set; }
    }
}