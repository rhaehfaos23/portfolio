using DearManager.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DearManager.ViewModel
{
    class PageService : IServicePage
    {
        private static PageService _default = new PageService();
        public static PageService Default
        {
            get { return _default; }
        }

        private PageService()
        {

        }

        public Task PushAsync(Page page)
        {
            return App.Current.MainPage.Navigation.PushAsync(page);
        }

        public Task PushModalAsync(Page page)
        {
            return App.Current.MainPage.Navigation.PushModalAsync(page);
        }

        public Task PopModalAsync()
        {
            return App.Current.MainPage.Navigation.PopModalAsync();
        }

        public Task DisplayAlert(string title, string message, string cancel)
        {
            return App.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return App.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }

        public Task PopAsync()
        {
            return App.Current.MainPage.Navigation.PopAsync();
        }

        public async Task<ImageSource> GetProfileImage(uint userId)
        {
            HttpUsersImagesAccess imagesAccess = new HttpUsersImagesAccess(userId);
            var data = await imagesAccess.HttpGetAsync();
            ImageSource image;
            if (data == null)
                image = ImageSource.FromFile("image_default_profile.png");
            else
                image = ImageSource.FromStream(() => data);
            return image;
        }
    }
}
