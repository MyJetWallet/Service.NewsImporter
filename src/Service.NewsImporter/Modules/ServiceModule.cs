using Autofac;
using Service.NewsImporter.Domain;
using Service.NewsImporter.Domain.ExternalSources;
using Service.NewsImporter.Jobs;
using Service.NewsImporter.Services;
using Service.NewsImporter.Services.ExternalSources;

namespace Service.NewsImporter.Modules
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<NewsImportJob>()
                .As<IStartable>()
                .AutoActivate()
                .SingleInstance();

            builder
                .RegisterType<NewsImportManager>()
                .As<INewsImportManager>()
                .SingleInstance();
            
            builder
                .RegisterType<ExternalNewsImporter>()
                .As<IExternalNewsImporter>()
                .SingleInstance();
            
            builder
                .RegisterType<StockNewsImporter>()
                .As<IStockNewsImporter>()
                .SingleInstance();
            
            builder
                .RegisterType<NewsStorage>()
                .As<INewsStorage>()
                .SingleInstance();
            
            builder
                .RegisterType<TickerStorage>()
                .As<ITickerStorage>()
                .SingleInstance();
        }
    }
}