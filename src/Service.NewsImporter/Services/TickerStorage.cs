using System.Collections.Generic;
using System.Threading.Tasks;
using Service.NewsImporter.Domain;

namespace Service.NewsImporter.Services
{
    public class TickerStorage : ITickerStorage
    {
        public async Task<List<string>> GetTickers()
        {
            return new List<string>()
            {
                {"BTC"}
            };
        }
    }
}