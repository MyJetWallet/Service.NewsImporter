using System.Threading.Tasks;

namespace Service.NewsImporter.Domain
{
    public interface IExternalNewsImporter
    {
        Task GetNewsAsync();
    }
}