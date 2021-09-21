using System.Collections.Generic;
using System.Runtime.Serialization;
using Service.NewsImporter.Domain.NoSql;

namespace Service.NewsImporter.Grpc.Models
{
    [DataContract]
    public class UpdateTikerSettingsResponse
    {
        [DataMember(Order = 1)] public bool Success { get; set; }
        [DataMember(Order = 2)] public string ErrorText { get; set; }
        [DataMember(Order = 3)] public List<ExternalTickerSettings> SettingsCollection { get; set; }
    }
}