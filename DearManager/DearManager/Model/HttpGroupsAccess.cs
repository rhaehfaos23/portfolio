using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DearManager.Accessible;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace DearManager.Model
{
    class HttpGroupsAccess : HttpAccesser, ICanHttpMethodPost<Group>
    {
        private Uri uri;
        public HttpGroupsAccess(): base()
        {
            uri = new Uri(HttpUrl.GetGroupsUrl());
        }

        /// <summary>
        /// 그룹 추가
        /// </summary>
        /// <param name="data">json(name, admin_id)</param>
        /// <returns></returns>
        public async Task<Group> HttpPostAsync(string data)
        {
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var res = await Http.PostAsync(uri, content);
            res.EnsureSuccessStatusCode();
            var responedResult = await res.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Group>(responedResult);
            return result;
        }
    }
}
