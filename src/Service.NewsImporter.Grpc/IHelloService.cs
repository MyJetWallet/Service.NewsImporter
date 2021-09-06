using System.ServiceModel;
using System.Threading.Tasks;
using Service.NewsImporter.Grpc.Models;

namespace Service.NewsImporter.Grpc
{
    [ServiceContract]
    public interface IHelloService
    {
        [OperationContract]
        Task<HelloMessage> SayHelloAsync(HelloRequest request);
    }
}