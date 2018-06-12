using DearManager.Model;
using DearManager.ViewModel;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DearManager.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
        private Person _friend = null;
        private ProfileViewModel ViewModel
        {
            get => BindingContext as ProfileViewModel;
            set => BindingContext = value;
        }
        private string profilePath;

        //자신의 프로필화면을 보는경우
		public ProfilePage ()
		{
			InitializeComponent ();
            if (ViewModel == null)
            {
                ViewModel = new ProfileViewModel();
            }
        }

        //친구의 프로필 화면을 보는경우
        public ProfilePage(Person friend): this()
        {
            _friend = friend;
            Title = friend.Name + AppResources.txtProfilePageProfileTitleUnit;

            //친구 프로필 화면인 경우 친구 삭제버튼 추가
            Button button = new Button { Text = AppResources.txtProfilePageFriendRemoveButton, Style = (Style) Application.Current.Resources["okButtonStyle"], BackgroundColor=Color.Transparent,
                WidthRequest = 100, CommandParameter = friend.Id};
            button.SetBinding(Button.CommandProperty, nameof(ProfileViewModel.CRemoveFriend));
            relativeLayout.Children.Add(button, yConstraint: Constraint.RelativeToParent((parent) =>
            {
                return (0.7 * parent.Height) - 15;
            }), xConstraint: Constraint.RelativeToParent(parent =>
            {
                return (0.5 * parent.Width) - 50;
            }));
            ViewModel = new ProfileViewModel(friend);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            profilePath = $"Profile_{(uint)Application.Current.Properties[App.userId]}.bin";
            (BindingContext as ProfileViewModel).GetUserInfo();
            if (_friend == null)
                ImageSetDefaultOrSaved();
            else
                profileImage.Source = _friend.ProfileImage;
        }

        private void ImageSetDefaultOrSaved()
        {
            var data = DependencyService.Get<Extension.ISaveAndLoad>().LoadImage(profilePath);
            if (data == null)
            {
                profileImage.Source = ImageSource.FromFile("image_default_profile.png");
            }
            else
            {
                var ms = new MemoryStream(data, 0, data.Length);
                ImageSource image = ImageSource.FromStream(() => ms);
                profileImage.Source = image;
            }
        }

        async void OnTap(object sender, EventArgs e)
        {
            string action;

            // 권한 확인 후 다이얼로그 생성
            if (_friend == null)
            {
                action = await DisplayActionSheet(AppResources.txtProfilePageDialogTitle, AppResources.txtProfilePageDialogCancel, 
                    AppResources.txtProfilePageDialogRemovePhoto, AppResources.txtProfilePageDialogShowBigPhoto,
                    AppResources.txtProfilePageDialogChangePhoto);

                if (action == AppResources.txtProfilePageDialogChangePhoto)
                    ChangeProfilePicture();
                else if (action == AppResources.txtProfilePageDialogRemovePhoto)
                    DeleteProfilePicture();
                else if (action == AppResources.txtProfilePageDialogShowBigPhoto)
                    await PageService.Default.PushAsync(new ProfilePicturePage((uint)Application.Current.Properties[App.userId]));
            }
            else
            {
                await PageService.Default.PushAsync(new ProfilePicturePage(_friend.Id));
            }
        }

        //프로필 사진 삭제기능
        async void DeleteProfilePicture()
        {
            DependencyService.Get<Extension.ISaveAndLoad>().DeleteImage(profilePath);
            HttpUsersImagesAccess imagesAccess = new HttpUsersImagesAccess((uint)Application.Current.Properties[App.userId]);
            await imagesAccess.HttpDeleteAsync(null);
            ImageSetDefaultOrSaved();
        }

        //프로필 사진 변경
        async void ChangeProfilePicture()
        {
            await CrossMedia.Current.Initialize();
            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                try
                {
                    var image = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions() { CompressionQuality = 95, MaxWidthHeight = 1000 });
                    Stream stream = image.GetStream();
                    if (stream != null)
                    {
                        byte[] temp = null;
                        using (var ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            temp = ms.ToArray();
                            stream.Dispose();
                            ms.Dispose();
                        }
                        DependencyService.Get<Extension.ISaveAndLoad>().SaveImage(App.tempImage, temp);

                        await PageService.Default.PushAsync(new CropImagePage(profilePath));
                        ImageSetDefaultOrSaved();
                    }
                    else
                    {
                        ImageSetDefaultOrSaved();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}