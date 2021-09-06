using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Service.NewsImporter.Domain;
using Service.NewsImporter.Jobs;
using Service.NewsImporter.Services;

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
        }
    }
}