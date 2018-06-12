using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DearManager.Accessible;
using DearManager.ViewModel;

namespace DearManager.Model
{
    class HttpUsersImagesAccess : HttpAccesser, ICanHttpMethodDelete, ICanHttpMethodPost, ICanHttpMethodPut, ICanHttpMethodGet<Stream>
    {
        Uri uri;

        /// <summary>
        /// URL 생성
        /// URL　生成
        /// </summary>
        /// <param name="usedId"></param>
        public HttpUsersImagesAccess(uint usedId) : base()
        {
            uri = new Uri(HttpUrl.GetUsersImagesUrl(usedId));
        }

        /// <summary>
        /// 유저 프로필 이미지 삭제
        /// ユーザーのプロファイルイメージ削除
        /// </summary>
        public Task HttpDeleteAsync(string data　)
        {
            return Http.DeleteAsync(uri);
        }

        /// <summary>
        /// 유저 프로필 이미지 불러오기
        /// ユーザーのプロファイルイメージを読み込む
        /// </summary>
        public async Task<Stream> HttpGetAsync()
        {
            try
            {
                var data = await Http.GetStreamAsync(uri);
                return data;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }


        /// <summary>
        /// 유저 프로필 이미지 저장
        /// ユーザープロファイルイメージ保存
        /// </summary>
        /// <param name="data">json(image_byte)</param>
        public async Task HttpPostAsync(string data)
        {
            StringContent stringContent = new StringContent(data, Encoding.UTF8, "application/json");
            var res = await Http.PostAsync(uri, stringContent);
            res.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// 유저 프로필 이미지 변경
        /// ユーザープロファイルイメージ変更
        /// </summary>
        /// <param name="data">json(image_byte)</param>
        public async Task HttpPutAsync(string data)
        {
            StringContent stringContent = new StringContent(data, Encoding.UTF8, "application/json");
            var res = await Http.PutAsync(uri, stringContent);
            res.EnsureSuccessStatusCode();
        }
    }
}
