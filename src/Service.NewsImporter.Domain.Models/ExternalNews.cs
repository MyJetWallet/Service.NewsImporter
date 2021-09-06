using System;
using System.Collections.Generic;

namespace Service.NewsImporter.Domain.Models
{
    public class ExternalNews
    {
        public string Title { get; set; }
        public string NewsUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Source { get; set; }
        public DateTime Date { get; set; }
        public string Sentiment { get; set; }
        public List<string> ExternalTickers { get; set; }
        public List<string> InternalTickers { get; set; }
    }
}