using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DearManager.Accessible;
using Newtonsoft.Json;

namespace DearManager.Model
{
    class HttpUsersInfoAccesser : HttpAccesser, ICanHttpMethodGet<Person>, ICanHttpMethodDelete, ICanHttpMethodPut
    {
        private Uri uri;
        public HttpUsersInfoAccesser(uint userId)
        {
            uri = new Uri(HttpUrl.GetUsersInfoUrl(userId.ToString()));
        }

        public HttpUsersInfoAccesser(string nickName)
        {
            uri = new Uri(HttpUrl.GetUsersInfoUrl(nickName));
        }

        /// <summary>
        /// 유저 정보 삭제하기
        /// </summary>
        /// <param name="data"></param>
        public Task HttpDeleteAsync(string data = null)
        {
            return Http.DeleteAsync(uri);
        }

        /// <summary>
        /// 유저 정보 받아오기
        /// </summary>
        /// <returns>유저 정보</returns>
        public async Task<Person> HttpGetAsync()
        {
            UriBuilder uBuilder = new UriBuilder(uri)
            {
                Query = "isNickname=false"
            };

            var getResult = await Http.GetStringAsync(uBuilder.Uri);
            return JsonConvert.DeserializeObject<Person>(getResult);
        }

        public async Task<bool> HttpGetIsAlready(string nickName)
        {
            UriBuilder uBuilder = new UriBuilder(uri)
            {
                Query = $"isNickname=true&nickname={nickName}"
            };

            var getResult = await Http.GetStringAsync(uBuilder.Uri);
            var result = getResult.Equals("true") ? true : false;
            return result;
        }

        /// <summary>
        /// 유저 정보 변경하기
        /// </summary>
        /// <param name="data">json(nickname, profile_image)</param>
        public async Task HttpPutAsync(string data)
        {
            StringContent stringContent = new StringContent(data, Encoding.UTF8, "application/json");
            var res = await Http.PutAsync(uri, stringContent);
            res.EnsureSuccessStatusCode();
        }
    }
}
