using DearManager.Model;
using DearManager.ViewModel;
using FFImageLoading.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DearManager.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CropImagePage : ContentPage
	{
        private string _profilePath;
		public CropImagePage (string profilePath)
		{
			InitializeComponent ();

            _profilePath = profilePath;
            BindingContext = new CropImageViewModel();
        }

        async protected override void OnAppearing()
        {
            await CachedImage.InvalidateCache(cropView.Source, FFImageLoading.Cache.CacheType.All, true);

            base.OnAppearing();
        }

        async protected override void OnDisappearing()
        {
            await CachedImage.InvalidateCache(cropView.Source, FFImageLoading.Cache.CacheType.All, true);

            base.OnDisappearing();
        }

        /// <summary>
        /// 이미지 저장
        /// イメージ保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void ImageSave(object sender, EventArgs e)
        {
            var result = await cropView.GetImageAsJpegAsync();
            byte[] vs = null;

            //stream을 byte로 변환
            //streamをbyteに変換
            using (var ms = new MemoryStream())
            {
                result.CopyTo(ms);
                vs = ms.ToArray();
                result.Dispose();
                ms.Dispose();
            }

            //로컬 저장소에 저장
            //ロカールに保存
            DependencyService.Get<Extension.ISaveAndLoad>().SaveImage(_profilePath, vs);
            //임시로 저장한 이미지 삭제
            //臨時に保存したイメージ削除
            DependencyService.Get<Extension.ISaveAndLoad>().DeleteImage(App.tempImage);

            //현재 창 끔
            //今のページを閉じる
            await PageService.Default.PopAsync();

            //이미지를 저장
            //イメージをS3に保存
            HttpUsersImagesAccess imagesAccess = new HttpUsersImagesAccess((uint)Application.Current.Properties[App.userId]);
            var b64string = Convert.ToBase64String(vs);
            var temp2 = new { image_byte = b64string };
            var data = JsonConvert.SerializeObject(temp2);
            try
            {
                await imagesAccess.HttpPostAsync(data);
            }
            catch
            {
                //로컬라이즈
                //이미지 업로드 실패하면 경고창 띄움 그리고 저장한 이미지 로컬저장소에서 삭제
                //アップロードを失敗すると警告そのあとローカルに保存したイメージ削除
                await PageService.Default.DisplayAlert("업로드 실패", "이미지를 업로드 실패하였습니다.", "확인");
                DependencyService.Get<Extension.ISaveAndLoad>().DeleteImage(_profilePath);
            }
        }

        async void ImageCancel(object sender, EventArgs e)
        {
            DependencyService.Get<Extension.ISaveAndLoad>().DeleteImage(App.tempImage);
            await PageService.Default.PopAsync();
        }
    }
}