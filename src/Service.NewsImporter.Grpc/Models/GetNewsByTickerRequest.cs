using System.Runtime.Serialization;

namespace Service.NewsImporter.Grpc.Models
{
    [DataContract]
    public class GetNewsByTickerRequest
    {
        [DataMember(Order = 1)] public string Ticker { get; set; }
    }
}