using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DearManager.Accessible;
using Newtonsoft.Json;

namespace DearManager.Model
{
    class HttpUsersGroupsAccess : HttpAccesser, ICanHttpMethodGet<List<Group>>
    {
        private Uri uri;
        public HttpUsersGroupsAccess(uint userId) : base()
        {
            uri = new Uri(HttpUrl.GetUsersGroupsUrl(userId));
        }

        public async Task<List<Group>> HttpGetAsync()
        {
            var result = await Http.GetStringAsync(uri);
            return JsonConvert.DeserializeObject<List<Group>>(result);
        }
    }
}
