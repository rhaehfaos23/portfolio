using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DearManager.Accessible;
using System.Net.Http;
using Newtonsoft.Json;

namespace DearManager.Model
{
    class HttpUsersFriendsAccess : HttpAccesser, ICanHttpMethodGet<List<Person>>, ICanHttpMethodDelete, ICanHttpMethodPost
    {
        private Uri uri;
        public HttpUsersFriendsAccess(uint userId): base()
        {
            uri = new Uri(HttpUrl.GetUsersFriendsUrl(userId));
        }

        /// <summary>
        /// 유저의 친구 정보 삭제
        /// </summary>
        /// <param name="data">json(my_id, friend_id)</param>
        public async Task HttpDeleteAsync(string data)
        {
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpRequestMessage httpRequest = new HttpRequestMessage()
            {
                Content = content,
                Method = HttpMethod.Delete,
                RequestUri = uri
            };

            await Http.SendAsync(httpRequest);
        }

        /// <summary>
        /// 유저의 친구목록 불러오기
        /// </summary>
        /// <returns>친구목록json</returns>
        public async Task<List<Person>> HttpGetAsync()
        {
            var result = await Http.GetStringAsync(uri);
            return JsonConvert.DeserializeObject<List<Person>>(result);
        }

        /// <summary>
        /// 유저 친구 추가하기
        /// </summary>
        /// <param name="data">json(my_id, friend_id)</param>
        public async Task HttpPostAsync(string data)
        {
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await Http.PostAsync(uri, content);
            response.EnsureSuccessStatusCode();
        }
    }
}
