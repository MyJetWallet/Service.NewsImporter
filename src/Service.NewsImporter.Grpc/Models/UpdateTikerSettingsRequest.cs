using System.Runtime.Serialization;
using Service.NewsImporter.Domain.Models;
using Service.NewsImporter.Domain.NoSql;

namespace Service.NewsImporter.Grpc.Models
{
    [DataContract]
    public class UpdateTikerSettingsRequest
    {
        [DataMember(Order = 1)] public ExternalTickerSettings Settings { get; set; }
    }
}