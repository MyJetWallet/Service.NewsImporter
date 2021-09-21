using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.NewsImporter.Domain.NoSql;
using Service.NewsImporter.Grpc;
using Service.NewsImporter.Grpc.Models;

namespace Service.NewsImporter.Services
{
    public class ExternalTickerSettingsService : IExternalTickerSettingsService
    {
        private readonly IExternalTickerSettingsStorage _externalTickerSettingsStorage;
        private readonly ILogger<ExternalTickerSettingsService> _logger;

        public ExternalTickerSettingsService(IExternalTickerSettingsStorage externalTickerSettingsStorage,
            ILogger<ExternalTickerSettingsService> logger)
        {
            _externalTickerSettingsStorage = externalTickerSettingsStorage;
            _logger = logger;
        }

        public async Task<GetTikerSettingsResponse> GetTikerSettingsAsync()
        {
            try
            {
                var settings = _externalTickerSettingsStorage.GetExternalTickerSettings();

                return new GetTikerSettingsResponse()
                {
                    Success = true,
                    SettingsCollection = settings
                };
            }
            catch (Exception ex)
            {
                return new GetTikerSettingsResponse()
                {
                    Success = false,
                    ErrorText = ex.Message
                };
            }
        }

        public async Task<UpdateTikerSettingsResponse> UpdateTikerSettingsAsync(UpdateTikerSettingsRequest request)
        {
            try
            {
                _logger.LogInformation("Update tiker settings: {requestJson}", JsonConvert.SerializeObject(request));

                if (!string.IsNullOrEmpty(request.Settings.NewsTicker))
                {
                    await _externalTickerSettingsStorage.UpdateExternalTickerSettingsAsync(request.Settings);
                }

                var settings = _externalTickerSettingsStorage.GetExternalTickerSettings();
                return new UpdateTikerSettingsResponse()
                {
                    Success = true,
                    SettingsCollection = settings
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot update tiker settings");
                
                return new UpdateTikerSettingsResponse()
                {
                    Success = false,
                    ErrorText = ex.Message
                };
            }
        }
    }
}