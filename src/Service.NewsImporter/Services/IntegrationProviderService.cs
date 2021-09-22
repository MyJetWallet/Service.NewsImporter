using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.NewsImporter.Domain;
using Service.NewsImporter.Domain.Models;
using Service.NewsImporter.Grpc;
using Service.NewsImporter.Grpc.Models;

namespace Service.NewsImporter.Services
{
    public class IntegrationProviderService : IIntegrationProviderService
    {
        private readonly IExternalNewsImporter _externalNewsImporter;
        private readonly ILogger<IntegrationProviderService> _logger;

        public IntegrationProviderService(IExternalNewsImporter externalNewsImporter,
            ILogger<IntegrationProviderService> logger)
        {
            _externalNewsImporter = externalNewsImporter;
            _logger = logger;
        }

        public async Task<GetNewsByTickerResponse> GetNewsByTickerAsync(GetNewsByTickerRequest request)
        {
            try
            {
                _logger.LogInformation("GetNewsByTickerAsync received request: {requestJson}.", JsonConvert.SerializeObject(request));

                if (string.IsNullOrWhiteSpace(request.Ticker))
                {
                    return new GetNewsByTickerResponse()
                    {
                        Success = false,
                        ErrorText = "Empty request ticker"
                    };
                }
                var news = await _externalNewsImporter.GetNewsAsync(new List<ExternalTickerSettings>(){new ExternalTickerSettings()
                {
                    NewsTicker = request.Ticker,
                    IntegrationSource = "StockNews"
                }}, true);

                _logger.LogInformation("ExternalNewsImporter find {newsCount} news for {requestTicker}.", news.Count, request.Ticker);
                return new GetNewsByTickerResponse()
                {
                    Success = true,
                    News = news
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetNewsByTickerAsync catch ex: {ex.Message}", ex);
                return new GetNewsByTickerResponse()
                {
                    Success = false,
                    ErrorText = ex.Message
                };
            }
        }
    }
}