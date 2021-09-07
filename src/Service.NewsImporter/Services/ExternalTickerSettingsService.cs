using System;
using System.Threading.Tasks;
using Service.NewsImporter.Domain.NoSql;
using Service.NewsImporter.Grpc;

namespace Service.NewsImporter.Services
{
    public class ExternalTickerSettingsService : IExternalTickerSettingsService
    {
        private readonly IExternalTickerSettingsStorage _externalTickerSettingsStorage;

        public ExternalTickerSettingsService(IExternalTickerSettingsStorage externalTickerSettingsStorage)
        {
            _externalTickerSettingsStorage = externalTickerSettingsStorage;
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
                await _externalTickerSettingsStorage.UpdateExternalTickerSettingsAsync(request.Settings);
                var settings = _externalTickerSettingsStorage.GetExternalTickerSettings();
                return new UpdateTikerSettingsResponse()
                {
                    Success = true,
                    SettingsCollection = settings
                };
            }
            catch (Exception ex)
            {
                return new UpdateTikerSettingsResponse()
                {
                    Success = false,
                    ErrorText = ex.Message
                };
            }
        }
    }
}