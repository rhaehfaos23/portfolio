using DearManager.Accessible;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DearManager.Model
{
    class HttpUsersRequestAccess
        : HttpAccesser, ICanHttpMethodDelete, ICanHttpMethodPost, ICanHttpMethodGet<List<Person>>
    {
        private Uri uri;

        public HttpUsersRequestAccess(uint userId): base()
        {
            uri = new Uri(HttpUrl.GetUsersRequestUrl(userId));
        }

        public Task HttpDeleteAsync(string data)
        {
            HttpRequestMessage message = new HttpRequestMessage
            {
                Content = new StringContent(data, Encoding.UTF8, "application/json"),
                Method = HttpMethod.Delete,
                RequestUri = uri
            };

            return Http.SendAsync(message);
        }

        public async Task<List<Person>> HttpGetAsync()
        {
            var result = await Http.GetStringAsync(uri);
            return JsonConvert.DeserializeObject<List<Person>>(result);
        }


        /// <summary>
        /// 친구 신청
        /// </summary>
        /// <param name="data">json(my_id, friend_id)</param>
        public async Task HttpPostAsync(string data)
        {
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var res = await Http.PostAsync(uri, content);
            res.EnsureSuccessStatusCode();
        }
    }
}
