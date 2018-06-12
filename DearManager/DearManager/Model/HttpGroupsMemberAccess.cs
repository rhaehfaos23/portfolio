using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DearManager.Accessible;
using Newtonsoft.Json;

namespace DearManager.Model
{
    class HttpGroupsMemberAccess: HttpAccesser, ICanHttpMethodGet<List<Person>>, ICanHttpMethodPost, ICanHttpMethodDelete
    {
        private Uri uri;

        public HttpGroupsMemberAccess(uint groupId): base()
        {
            uri = new Uri(HttpUrl.GetGroupMemberUrl(groupId));
        }

        /// <summary>
        /// 멤버 목록 삭제
        /// </summary>
        /// <param name="data">json(person_id, group_id)</param>
        /// <returns></returns>
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

        /// <summary>
        /// 그룹 멤버 목록 불러오기
        /// </summary>
        /// <returns>그룹 멤버목록</returns>
        public async Task<List<Person>> HttpGetAsync()
        {
            var result = await Http.GetStringAsync(uri);
            return JsonConvert.DeserializeObject<List<Person>>(result);
        }

        /// <summary>
        /// 그룹에 멤버 추가
        /// </summary>
        /// <param name="data">json(person_id, group_id)</param>
        public async Task HttpPostAsync(string data)
        {
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var res = await Http.PostAsync(uri, content);
            res.EnsureSuccessStatusCode();
        }
    }
}
