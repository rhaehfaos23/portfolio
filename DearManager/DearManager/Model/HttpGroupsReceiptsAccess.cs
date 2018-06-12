using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DearManager.Accessible;
using Newtonsoft.Json;

namespace DearManager.Model
{
    class HttpGroupsReceiptsAccess : HttpAccesser, ICanHttpMethodGet<List<Receipt>>, ICanHttpMethodPost, ICanHttpMethodDelete
    {
        private Uri uri;

        /// <summary>
        /// 영수증을 불러올때 사용해야될 생성자
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize">불러올 영수증 갯수</param>
        public HttpGroupsReceiptsAccess(uint groupId, int page, int pageSize)
        {
            uri = new Uri(HttpUrl.GetGroupReceiptsUrl(groupId, page, pageSize));
        }

        /// <summary>
        /// 영수증을 불러올때는 사용하지 마시오
        /// </summary>
        /// <param name="groupId"></param>
        public HttpGroupsReceiptsAccess(uint groupId) : this(groupId, 0, 0)
        {

        }

        public async Task<List<Receipt>> HttpGetAsync()
        {
            var result = await Http.GetStringAsync(uri);
            return JsonConvert.DeserializeObject<List<Receipt>>(result);
        }

        /// <summary>
        /// 영수증 추가
        /// </summary>
        /// <param name="data">json(person_id, group_id, money, tag)</param>
        /// <returns></returns>
        public async Task HttpPostAsync(string data)
        {
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var res = await Http.PostAsync(uri, content);
            res.EnsureSuccessStatusCode();
        }

        public Task HttpDeleteAsync(string data)
        {
            HttpRequestMessage message = new HttpRequestMessage
            {
                Content = new StringContent(data),
                Method = HttpMethod.Delete,
                RequestUri = uri
            };
            return Http.SendAsync(message);
        }
    }
}
