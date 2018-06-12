using DearManager.Model;
using DearManager.ViewModel;
using System;
using Xamarin.Forms;

namespace DearManager
{
	public partial class App : Application
	{
        public const string userInfo = "UserInfo";
        public const string isFirst = "First";
        public const string userId = "UserId";
        public const string userName = "UserName";
        public const string userNickName = "UserNickName";
        public const string userImage = "UserImage";
        public const string currentAddedGroup = "CurrentAddedGroup";
        public const string confirmTutorialFirst = "TutorialFirst";
        public const string tempImage = "temp.bin";

        public App ()
		{
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                AppResources.Culture = ci; // set the RESX for resource localization
                DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
            }

            InitializeComponent();
            MainPage = new NavigationPage(new View.MainPage());
        }

		protected override void OnStart ()
		{
            if (Application.Current.Properties.ContainsKey(isFirst))
            {
                string first = (string)Application.Current.Properties[isFirst];
                bool boolFirst = Convert.ToBoolean(first);
                if (!boolFirst)
                {
                    return;
                }
            } 

            MainPage.Navigation.PushModalAsync(new View.InputUserPage());
        }

		protected override void OnSleep ()
		{

		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
