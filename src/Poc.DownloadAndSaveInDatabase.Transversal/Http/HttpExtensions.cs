using Poc.DownloadAndSaveInDatabase.Transversal.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Poc.DownloadAndSaveInDatabase.Transversal.Http
{
    public static class HttpExtensions
    {
        public static long ObtainContentLength(this HttpClient httpClient, Uri uri)
        {
         
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Head, uri);

            HttpResponseMessage response = httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result;

            long contentType = default(long);

            if (response.Content.Headers.TryGetValues(HttpConstants.HttpContentlengthHeader, out IEnumerable<string> headerValues))
            {
                long.TryParse(headerValues.FirstOrDefault(), out contentType);
            }

            return contentType;
        }
    }
}
