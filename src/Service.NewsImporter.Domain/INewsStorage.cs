using System.Threading.Tasks;

namespace Service.NewsImporter.Domain
{
    public interface INewsStorage
    {
        Task SaveNewsAsync();
    }
}