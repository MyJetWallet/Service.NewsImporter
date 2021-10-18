using Service.NewsImporter.Services.ExternalSources;

namespace Service.NewsImporter.Services
{
    public static class SentimentConvertor
    {
        private const string NeutralSentiment = "Neutral";
        private const string PositiveSentiment = "Positive";
        private const string NegativeSentiment = "Negative";

        public static string ConvertSentiment(CryptoPanicVotes cryptoPanicVotes)
        {
            if (cryptoPanicVotes.positive > (cryptoPanicVotes.negative + cryptoPanicVotes.toxic))
            {
                return PositiveSentiment;
            }
            if ((cryptoPanicVotes.negative + cryptoPanicVotes.toxic) > cryptoPanicVotes.positive)
            {
                return NegativeSentiment;
            }
            return NeutralSentiment;
        }
    }
}