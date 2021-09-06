using System.Collections.Generic;
using System.Threading.Tasks;
using Service.NewsImporter.Domain.Models;

namespace Service.NewsImporter.Domain
{
    public interface INewsStorage
    {
        Task SaveNewsAsync(List<News> news);
    }
}