using System.Threading.Tasks;

namespace Service.NewsImporter.Domain
{
    public interface INewsImportManager
    {
        Task HandleNewsAsync();
    }
}