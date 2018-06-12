using System;
using System.Threading.Tasks;
using DearManager.Accessible;
using Newtonsoft.Json;

namespace DearManager.Model
{
    class HttpGroupsInfoAccess : HttpAccesser, ICanHttpMethodGet<Group>, ICanHttpMethodDelete
    {
        private Uri uri;
        public HttpGroupsInfoAccess(uint groupId) : base()
        {
            uri = new Uri(HttpUrl.GetGroupInfoUrl(groupId));
        }

        public Task HttpDeleteAsync(string data)
        {
            return Http.DeleteAsync(uri);
        }

        public async Task<Group> HttpGetAsync()
        {
            var result = await Http.GetStringAsync(uri);
            return JsonConvert.DeserializeObject<Group>(result);
        }
    }
}
