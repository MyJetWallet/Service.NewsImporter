﻿using Autofac;
using Service.NewsImporter.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.NewsImporter.Client
{
    public static class AutofacHelper
    {
        public static void RegisterNewsImporterClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new NewsImporterClientFactory(grpcServiceUrl);
            builder.RegisterInstance(factory.GetAExternalTickerSettingsService()).As<IExternalTickerSettingsService>().SingleInstance();
        }
    }
}
