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
        
        public IExternalTickerSettingsService GetExternalTickerSettingsService() => CreateGrpcService<IExternalTickerSettingsService>();
        public IIntegrationProviderService GetIntegrationProviderService() => CreateGrpcService<IIntegrationProviderService>();
    }
}
