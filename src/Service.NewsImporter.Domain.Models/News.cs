using System;
using System.Collections.Generic;

namespace Service.NewsImporter.Domain.Models
{
    public class News
    {
        public string Title { get; set; }
        public string NewsUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Source { get; set; }
        public DateTime Date { get; set; }
        public string Sentiment { get; set; }
        public List<string> Tickers { get; set; }
    }
}