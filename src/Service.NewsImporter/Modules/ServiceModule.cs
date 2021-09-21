using Autofac;
using MyJetWallet.Sdk.NoSql;
using Service.NewsImporter.Domain;
using Service.NewsImporter.Domain.ExternalSources;
using Service.NewsImporter.Domain.NoSql;
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
                .RegisterType<CryptoPanicImporter>()
                .As<ICryptoPanicImporter>()
                .SingleInstance();
            
            builder
                .RegisterType<NewsStorage>()
                .As<INewsStorage>()
                .SingleInstance();
            
            builder
                .RegisterType<ExternalTickerSettingsStorage>()
                .As<IExternalTickerSettingsStorage>()
                .As<IStartable>()
                .AutoActivate()
                .SingleInstance();
            
            builder.RegisterMyNoSqlWriter<ExternalTickerSettingsNoSql>(Program.ReloadedSettings(e => e.MyNoSqlWriterUrl), ExternalTickerSettingsNoSql.TableName);
        }
    }
}