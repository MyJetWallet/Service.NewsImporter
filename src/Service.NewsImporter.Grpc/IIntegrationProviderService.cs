using System.ServiceModel;
using System.Threading.Tasks;
using Service.NewsImporter.Grpc.Models;

namespace Service.NewsImporter.Grpc
{
    [ServiceContract]
    public interface IIntegrationProviderService
    {
        [OperationContract]
        Task<GetNewsByTickerResponse> GetNewsByTickerAsync(GetNewsByTickerRequest request);
    }
}