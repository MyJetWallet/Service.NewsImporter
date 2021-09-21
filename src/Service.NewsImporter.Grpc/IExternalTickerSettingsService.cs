using System.ServiceModel;
using System.Threading.Tasks;
using Service.NewsImporter.Grpc.Models;
 
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
}