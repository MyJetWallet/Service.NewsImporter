using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Service.NewsImporter.Domain;
using Service.NewsImporter.Domain.Models;

namespace Service.NewsImporter.Services
{
    public class NewsStorage : INewsStorage
    {
        public async Task SaveNewsAsync(List<News> news)
        {
            foreach (var e in news)
            {
                Console.WriteLine(JsonConvert.SerializeObject(e));
            }
        }
    }
}