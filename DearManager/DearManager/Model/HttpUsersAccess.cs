using System;
using System.Text;
using DearManager.Accessible;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace DearManager.Model
{
    class HttpUsersAccess: HttpAccesser, ICanHttpMethodPost
    {
        private Uri uri;
        public HttpUsersAccess() : base()
        {
            uri = new Uri(HttpUrl.GetUsersUrl());
        }
        
        /// <summary>
        /// 유저 정보 추가
        /// 유저를 추가 후 앱에 최근 추가한 유저 정보 저장
        /// 이 정보가 현 유저 정보
        /// </summary>
        /// <param name="data">json(name, nickname, profile_image)</param>
        /// <returns></returns>
        public async Task HttpPostAsync(string data)
        {
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await Http.PostAsync(uri, content);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            Application.Current.Properties[App.userInfo] = result;
            var user = JsonConvert.DeserializeObject<Person>(result);
            SaveUser(user);
        }

        private void SaveUser(Person person)
        {
            var app = Application.Current;
            app.Properties[App.userId] = person.Id;
            app.Properties[App.userName] = person.Name;
            app.Properties[App.userNickName] = person.NickName;
            app.Properties[App.userImage] = person.ImagePath;
            app.SavePropertiesAsync();
        }
    }
}
