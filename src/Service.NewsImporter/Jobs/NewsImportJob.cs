using System;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Service.Tools;
using Service.NewsImporter.Domain;

namespace Service.NewsImporter.Jobs
{
    public class NewsImportJob : IStartable
    {
        private readonly ILogger<NewsImportJob> _logger;
        private readonly MyTaskTimer _timer;
        
        private readonly INewsImportManager _newsImportManager;

        public NewsImportJob(INewsImportManager newsImportManager, ILogger<NewsImportJob> logger)
        {
            _newsImportManager = newsImportManager;
            _logger = logger;
            _timer = new MyTaskTimer(nameof(NewsImportJob), TimeSpan.FromSeconds(Program.Settings.NewsImportTimerInSec), _logger, DoTime);

        }

        private async Task DoTime()
        {
            await _newsImportManager.HandleNewsAsync();
        }

        public void Start()
        {
            _timer.Start();
        }
    }
}