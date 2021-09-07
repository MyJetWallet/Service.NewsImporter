using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;
using Service.NewsImporter.Domain.NoSql;

namespace Service.NewsImporter.Grpc
{
    [ServiceContract]
    public interface IExternalTickerSettingsService
    {
        [OperationContract]
        Task<GetTikerSettingsResponse> GetTikerSettingsAsync();
        
        [OperationContract]
        Task<UpdateTikerSettingsResponse> UpdateTikerSettingsAsync(UpdateTikerSettingsRequest request);
    }

    [DataContract]
    public class UpdateTikerSettingsRequest
    {
        [DataMember(Order = 1)] public ExternalTickerSettings Settings { get; set; }
    }
    
    [DataContract]
    public class UpdateTikerSettingsResponse
    {
        [DataMember(Order = 1)] public bool Success { get; set; }
        [DataMember(Order = 2)] public string ErrorText { get; set; }
        [DataMember(Order = 3)] public List<ExternalTickerSettings> SettingsCollection { get; set; }
    }
    
    [DataContract]
    public class GetTikerSettingsResponse
    {
        [DataMember(Order = 1)] public bool Success { get; set; }
        [DataMember(Order = 2)] public string ErrorText { get; set; }
        [DataMember(Order = 3)] public List<ExternalTickerSettings> SettingsCollection { get; set; }
    }
}