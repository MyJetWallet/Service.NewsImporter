using JetBrains.Annotations;
using MyJetWallet.Sdk.Grpc;
using Service.NewsImporter.Grpc;

namespace Service.NewsImporter.Client
{
    [UsedImplicitly]
    public class NewsImporterClientFactory: MyGrpcClientFactory
    {
        public NewsImporterClientFactory(string grpcServiceUrl) : base(grpcServiceUrl)
        {
        }

        public IHelloService GetHelloService() => CreateGrpcService<IHelloService>();
    }
}
