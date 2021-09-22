using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Service.NewsImporter.Domain.Models
{
    [DataContract]
    public class ExternalNews
    {
        [DataMember(Order = 1)] public string Title { get; set; }
        [DataMember(Order = 2)] public string Description { get; set; }
        [DataMember(Order = 3)] public string NewsUrl { get; set; }
        [DataMember(Order = 4)] public string ImageUrl { get; set; }
        [DataMember(Order = 5)] public string Source { get; set; }
        [DataMember(Order = 6)] public DateTime Date { get; set; }
        [DataMember(Order = 7)] public string Sentiment { get; set; }
        [DataMember(Order = 8)] public List<string> ExternalTickers { get; set; }
        [DataMember(Order = 9)] public List<string> InternalTickers { get; set; }
        [DataMember(Order = 10)] public string IntegrationSource { get; set; }
        [DataMember(Order = 11)] public string Lang { get; set; }
    }
}