namespace Poc.DownloadAndSaveInDatabase.Transversal.Http
{
    using System;
    using System.Net.Http;

    public class GenericHttpClient
    {
        HttpClient httpClient { get; }

        public GenericHttpClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public long GetSizeOFile(Uri url)
        {
            var contenTypeValue = this.httpClient.ObtainContentLength(url);
           
            return contenTypeValue;
        }

    }
}
