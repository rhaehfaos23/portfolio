using System;
using System.Net.Http;

namespace DearManager.Model
{
    class HttpAccesser
    {
        protected HttpClient Http { get; set; }
        protected HttpAccesser()
        {
            Http = new HttpClient();
            Http.DefaultRequestHeaders.Add("x-api-key", Statics.apiKey);
            Http.Timeout = TimeSpan.FromMilliseconds(100000);
        }
    }
}
