﻿using Autofac;

// ReSharper disable UnusedMember.Global

namespace Service.NewsImporter.Client
{
    public static class AutofacHelper
    {
        public static void RegisterNewsImporterClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new NewsImporterClientFactory(grpcServiceUrl);
        }
    }
}
