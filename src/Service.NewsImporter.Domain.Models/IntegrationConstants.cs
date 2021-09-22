using System.Collections.Generic;

namespace Service.NewsImporter.Domain.Models
{
    public class IntegrationConstants
    {
        public static List<string> IntegrationSources = new List<string>() {"StockNews", "CryptoPanic"};
    }
}