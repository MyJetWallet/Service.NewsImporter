using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.NewsImporter.Domain
{
    public interface ITickerStorage
    {
        Task<List<string>> GetTickers();
    }
}