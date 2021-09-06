using Autofac;
using Service.NewsRepository.Client;

namespace Service.NewsImporter.Modules
{
    public class ClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterNewsRepositoryClient(Program.Settings.NewsRepositoryGrpcServiceUrl);
        }
    }
}