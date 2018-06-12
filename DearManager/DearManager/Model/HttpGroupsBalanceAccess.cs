using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DearManager.Accessible;
using Newtonsoft.Json;

namespace DearManager.Model
{
    class HttpGroupsBalanceAccess : HttpAccesser, ICanHttpMethodGet<List<BalanceInfo>>
    {
        private Uri uri;
        public HttpGroupsBalanceAccess(uint groupId) : base()
        {
            uri = new Uri(HttpUrl.GetGroupBalanceUrl(groupId));
        }

        public async Task<List<BalanceInfo>> HttpGetAsync()
        {
            var result = await Http.GetStringAsync(uri);
            return JsonConvert.DeserializeObject<List<BalanceInfo>>(result);
        }
    }
}
