using JetBrains.Annotations;
using MyJetWallet.Sdk.Grpc;

namespace Service.NewsImporter.Client
{
    [UsedImplicitly]
    public class NewsImporterClientFactory: MyGrpcClientFactory
    {
        public NewsImporterClientFactory(string grpcServiceUrl) : base(grpcServiceUrl)
        {
        }
    }
}
