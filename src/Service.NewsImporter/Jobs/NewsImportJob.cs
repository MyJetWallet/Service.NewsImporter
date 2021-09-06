using Service.NewsImporter.Domain;

namespace Service.NewsImporter.Jobs
{
    public class NewsImportJob
    {
        private readonly INewsImportManager _newsImportManager;

        public NewsImportJob(INewsImportManager newsImportManager)
        {
            _newsImportManager = newsImportManager;
        }
    }
}