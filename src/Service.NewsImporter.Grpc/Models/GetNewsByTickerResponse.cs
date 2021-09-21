using System.Collections.Generic;
using System.Runtime.Serialization;
using Service.NewsImporter.Domain.Models;

namespace Service.NewsImporter.Grpc.Models
{
    [DataContract]
    public class GetNewsByTickerResponse
    {
        [DataMember(Order = 1)] public bool Success { get; set; }
        [DataMember(Order = 2)] public string ErrorText { get; set; }
        [DataMember(Order = 3)] public List<ExternalNews> News { get; set; }
    }
}